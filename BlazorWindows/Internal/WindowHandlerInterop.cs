using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using KST.Blazor.Windows.Abstractions;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace KST.Blazor.Windows.Internal
{
	/// <summary>
	/// Javascript interop service
	/// </summary>
	public class WindowHandlerInterop : IAsyncDisposable
	{
		/// <summary>
		/// Javascript callback functions
		/// </summary>
		public class WindowManagementCallbacks
		{
			/// <summary>
			/// Object used to transfer screen information from javascript
			/// </summary>
			public record ScreenInfo(int Left, int Top, int Width, int Height, bool IsPrimary);

			private readonly WindowManagementImpl aWindowManagement;

			internal WindowManagementCallbacks(WindowManagementImpl windowManagement)
			{
				this.aWindowManagement = windowManagement;
			}

			/// <summary>
			/// Javascript detected window close
			/// </summary>
			/// <param name="id">Window id</param>
			[JSInvokable]
			public void OnWindowClosed(string id)
			{
				this.aWindowManagement.OnWindowClosed(Guid.Parse(id));
			}

			/// <summary>
			/// Javascript detected screen setup changes
			/// </summary>
			/// <param name="screens">Modified screen information</param>
			[JSInvokable]
			public void OnScreensChanged(ScreenInfo[] screens)
			{
				this.aWindowManagement.OnScreensChanged(screens.Select(x => new ScreenImpl(x.Left, x.Top, x.Width, x.Height, x.IsPrimary)));
			}
		}

		private readonly Lazy<Task<IJSObjectReference>> aModule;

		/// <summary>
		/// Initializes the javascript interop service
		/// </summary>
		/// <param name="jsRuntime">Javascript runtime used to communicate with javascript module</param>
		public WindowHandlerInterop(IJSRuntime jsRuntime)
		{
			this.aModule = new Lazy<Task<IJSObjectReference>>(async () => await jsRuntime.InvokeAsync<IJSObjectReference>("import", $"/_content/{Assembly.GetExecutingAssembly().GetName().Name}/WindowHandler.js"));
		}

		/// <summary>
		/// Opens a new window using javascript
		/// </summary>
		/// <param name="id">Chosen window ID</param>
		/// <param name="bodyElementRef">Reference to the element that should be shown as a window content</param>
		/// <param name="windowFeatures">Window features string</param>
		/// <param name="windowTitle">Initial window title</param>
		public async Task OpenWindow(Guid id, ElementReference bodyElementRef, string windowFeatures, string? windowTitle)
		{
			var module = await this.aModule.Value;
			await module.InvokeVoidAsync("OpenWindow", id.ToString(), bodyElementRef, windowFeatures, windowTitle);
		}

		/// <summary>
		/// Assigns window management service with javascript module for it to be able to call callbacks.
		/// The method could be called only once.
		/// </summary>
		/// <param name="windowManagement"></param>
		public async Task AssignWindowManagement(IWindowManagement windowManagement)
		{
			if (windowManagement is WindowManagementImpl impl)
			{
				var module = await this.aModule.Value;
				await module.InvokeVoidAsync("AssignWindowManagement", DotNetObjectReference.Create(new WindowManagementCallbacks(impl)));
			}
		}

		/// <summary>
		/// Changes the window title
		/// </summary>
		/// <param name="id">Window id</param>
		/// <param name="title">A new title</param>
		public async Task ChangeWindowTitle(Guid id, string title)
		{
			var module = await this.aModule.Value;
			await module.InvokeVoidAsync("ChangeWindowTitle", id.ToString(), title);
		}

		/// <summary>
		/// Initializes multi-screen window placement API if needed.
		/// The method could be called only once.
		/// </summary>
		/// <param name="enabled">True, if multi-screen window placement API should be enabled</param>
		public async Task SetMultiScreenWindowPlacement(bool enabled)
		{
			var module = await this.aModule.Value;
			await module.InvokeVoidAsync("SetMultiScreenWindowPlacement", enabled);
		}

		/// <inheritdoc />
		public async ValueTask DisposeAsync()
		{
			if (this.aModule.IsValueCreated)
				await (await this.aModule.Value).DisposeAsync();
		}
	}
}
