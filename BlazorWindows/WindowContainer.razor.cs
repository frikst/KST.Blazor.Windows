using System;
using Microsoft.AspNetCore.Components;

namespace KST.Blazor.Windows
{
	public partial class WindowContainer
	{
		[Inject]
		public IWindowManagement WindowManagement { get; set; }
			= default!;

		protected override void OnInitialized()
		{
			base.OnInitialized();

			if (this.WindowManagement is WindowManagementImpl impl)
			{
				impl.WindowsChanged += WindowManagementWindowsChanged;
			}
		}

		private void WindowManagementWindowsChanged(object? sender, EventArgs e)
		{
			_ = this.InvokeAsync(this.StateHasChanged);
		}
	}
}
