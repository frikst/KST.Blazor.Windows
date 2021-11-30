using System;
using System.Threading.Tasks;
using KST.Blazor.Windows.Internal;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace KST.Blazor.Windows
{
	public partial class WindowContainer
	{
		public class Callbacks
		{
			private WindowContainer aWindowContainer;

			internal Callbacks(WindowContainer windowContainer)
			{
				this.aWindowContainer = windowContainer;
			}

			[JSInvokable]
			public void OnWindowClosed(string id)
			{
				if (this.aWindowContainer.WindowManagement is WindowManagementImpl impl)
				{
					impl.OnWindowClosed(Guid.Parse(id));
				}
			}
		}

		[Inject]
		public IWindowManagement WindowManagement { get; set; }
			= default!;
		
		[Inject]
		public WindowHandlerInterop WindowHandler { get; set; }
			= default!;

		protected override void OnInitialized()
		{
			base.OnInitialized();

			if (this.WindowManagement is WindowManagementImpl impl)
			{
				impl.WindowsChanged += WindowManagementWindowsChanged;
			}
		}

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			await base.OnAfterRenderAsync(firstRender);

			if (firstRender)
				return;

			await this.WindowHandler.AssignWindowContainerCallbacks(new Callbacks(this));
		}

		private void WindowManagementWindowsChanged(object? sender, EventArgs e)
		{
			_ = this.InvokeAsync(this.StateHasChanged);
		}
	}
}
