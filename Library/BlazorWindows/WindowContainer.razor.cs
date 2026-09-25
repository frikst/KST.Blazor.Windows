using System;
using System.Threading.Tasks;
using KST.Blazor.Windows.Abstractions;
using KST.Blazor.Windows.Internal;
using KST.Blazor.Windows.Internal.Interop;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;

namespace KST.Blazor.Windows
{
	/// <summary>
	/// Container for window components. Should be instantiated once.
	/// </summary>
	public partial class WindowContainer
	{
		private bool aRequestWindowManagementPermission = false;

		/// <summary>
		/// Options for the KST.Blazor.Windows library
		/// </summary>
		[Inject]
		public IOptions<BlazorWindowOptions> Options { get; set; }
			= default!;

		/// <summary>
		/// Window management service
		/// </summary>
		[Inject]
		public IWindowManagement WindowManagement { get; set; }
			= default!;
		
		/// <summary>
		/// Javascript interop service
		/// </summary>
		[Inject]
		public WindowHandlerInterop WindowHandler { get; set; }
			= default!;

		/// <inheritdoc />
		protected override void OnInitialized()
		{
			base.OnInitialized();

			if (this.WindowManagement is WindowManagementImpl impl)
			{
				impl.WindowsChanged += WindowManagementWindowsChanged;
			}
		}

		/// <inheritdoc />
		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			await base.OnAfterRenderAsync(firstRender);

			if (!firstRender)
				return;

			await this.WindowHandler.AssignWindowManagementAsync(this.WindowManagement);

			if (this.Options.Value.EnableWindowManagementAPI)
			{
				var windowManagementPersmissionStatus = await this.WindowHandler.GetWindowManagementAPIStatusAsync();

				if (windowManagementPersmissionStatus == FeatureStatus.Allowed)
				{
					this.aRequestWindowManagementPermission = false;
					await this.WindowHandler.SetWindowManagementAPIAsync(true);
				}
				else
				{
					await this.WindowHandler.SetWindowManagementAPIAsync(false);

					if (windowManagementPersmissionStatus == FeatureStatus.Possible)
					{
						this.aRequestWindowManagementPermission = true;
						this.StateHasChanged();
					}
				}
			}
			else
			{
				await this.WindowHandler.SetWindowManagementAPIAsync(false);
			}
		}

		private void WindowManagementWindowsChanged(object? sender, EventArgs e)
		{
			_ = this.InvokeAsync(this.StateHasChanged);
		}

		private void DismissWindowManagementAPIPermission()
		{
			this.aRequestWindowManagementPermission = false;
		}

		private async Task AcceptWindowManagementAPIPermission()
		{
			this.aRequestWindowManagementPermission = false;
			await this.WindowHandler.SetWindowManagementAPIAsync(true);
		}
	}
}
