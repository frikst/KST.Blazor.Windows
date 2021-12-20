using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using KST.Blazor.Windows.Abstractions;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace KST.Blazor.Windows.Internal.Interop
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
		public async Task OpenWindowAsync(Guid id, ElementReference bodyElementRef, WindowFeatures windowFeatures, string? windowTitle)
		{
			var module = await this.aModule.Value;
			await module.InvokeVoidAsync("OpenWindow", id.ToString(), bodyElementRef, windowFeatures, windowTitle);
		}

		/// <summary>
		/// Assigns window management service with javascript module for it to be able to call callbacks.
		/// The method could be called only once.
		/// </summary>
		/// <param name="windowManagement"></param>
		public async Task AssignWindowManagementAsync(IWindowManagement windowManagement)
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
		public async Task ChangeWindowTitleAsync(Guid id, string title)
		{
			var module = await this.aModule.Value;
			await module.InvokeVoidAsync("ChangeWindowTitle", id.ToString(), title);
		}

		/// <summary>
		/// Closes window
		/// </summary>
		/// <param name="id">Window id</param>
		/// <returns></returns>
		public async Task CloseWindowAsync(Guid id)
		{
			var module = await this.aModule.Value;
			await module.InvokeVoidAsync("CloseWindow", id.ToString());
		}

		/// <summary>
		/// Returns information about multi-screen window placement API availability
		/// </summary>
		/// <returns></returns>
		public async Task<FeatureStatus> GetMultiScreenWindowPlacementStatusAsync()
		{
			var module = await this.aModule.Value;
			return await module.InvokeAsync<FeatureStatus>("GetMultiScreenWindowPlacementStatus");
		}

		/// <summary>
		/// Initializes multi-screen window placement API if needed.
		/// </summary>
		/// <param name="enabled">True, if multi-screen window placement API should be enabled</param>
		public async Task SetMultiScreenWindowPlacementAsync(bool enabled)
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
