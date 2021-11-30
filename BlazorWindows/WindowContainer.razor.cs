using System;
using System.Threading.Tasks;
using KST.Blazor.Windows.Internal;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace KST.Blazor.Windows
{
	public partial class WindowContainer
	{

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

			await this.WindowHandler.AssignWindowManagement(this.WindowManagement);
		}

		private void WindowManagementWindowsChanged(object? sender, EventArgs e)
		{
			_ = this.InvokeAsync(this.StateHasChanged);
		}
	}
}
