using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace KST.Blazor.Windows.Internal
{
	public class WindowHandlerInterop : IAsyncDisposable
	{
		public class WindowManagementCallbacks
		{
			public record ScreenInfo(int Left, int Top, int Width, int Height, bool IsPrimary);

			private readonly WindowManagementImpl aWindowManagement;

			internal WindowManagementCallbacks(WindowManagementImpl windowManagement)
			{
				this.aWindowManagement = windowManagement;
			}

			[JSInvokable]
			public void OnWindowClosed(string id)
			{
				this.aWindowManagement.OnWindowClosed(Guid.Parse(id));
			}

			[JSInvokable]
			public void OnScreensChanged(ScreenInfo[] screens)
			{
				this.aWindowManagement.OnScreensChanged(screens.Select(x => new ScreenImpl(x.Left, x.Top, x.Width, x.Height, x.IsPrimary)));
			}
		}

		private readonly Lazy<Task<IJSObjectReference>> aModule;

		public WindowHandlerInterop(IJSRuntime jsRuntime)
		{
			this.aModule = new Lazy<Task<IJSObjectReference>>(async () => await jsRuntime.InvokeAsync<IJSObjectReference>("import", $"/_content/{Assembly.GetExecutingAssembly().GetName().Name}/WindowHandler.js"));
		}

		public async Task OpenWindow(Guid id, ElementReference bodyElementRef, string windowFeatures, string? windowTitle)
		{
			var module = await this.aModule.Value;
			await module.InvokeVoidAsync("OpenWindow", id.ToString(), bodyElementRef, windowFeatures, windowTitle);
		}

		public async Task AssignWindowManagement(IWindowManagement windowManagement)
		{
			if (windowManagement is WindowManagementImpl impl)
			{
				var module = await this.aModule.Value;
				await module.InvokeVoidAsync("AssignWindowManagement", DotNetObjectReference.Create(new WindowManagementCallbacks(impl)));
			}
		}

		public async Task ChangeWindowTitle(Guid id, string title)
		{
			var module = await this.aModule.Value;
			await module.InvokeVoidAsync("ChangeWindowTitle", id.ToString(), title);
		}

		public async Task SetMultiScreenWindowPlacement(bool enabled)
		{
			var module = await this.aModule.Value;
			await module.InvokeVoidAsync("SetMultiScreenWindowPlacement", enabled);
		}

		public async ValueTask DisposeAsync()
		{
			if (this.aModule.IsValueCreated)
				await (await this.aModule.Value).DisposeAsync();
		}
	}
}
