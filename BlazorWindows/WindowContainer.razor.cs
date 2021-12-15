using System;
using System.Threading.Tasks;
using KST.Blazor.Windows.Abstractions;
using KST.Blazor.Windows.Internal;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;

namespace KST.Blazor.Windows
{
	/// <summary>
	/// Container for window components. Should be instantiated once.
	/// </summary>
	public partial class WindowContainer
	{
		private bool aRequestMultiScreenWindowPlacementPermission = false;

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

			if (this.Options.Value.EnableMultiScreenWindowPlacement)
			{
				var multiScreenWindowPlacementStatus = await this.WindowHandler.GetMultiScreenWindowPlacementStatusAsync();

				if (multiScreenWindowPlacementStatus == WindowHandlerInterop.FeatureStatus.Allowed)
				{
					this.aRequestMultiScreenWindowPlacementPermission = false;
					await this.WindowHandler.SetMultiScreenWindowPlacementAsync(true);
				}
				else
				{
					await this.WindowHandler.SetMultiScreenWindowPlacementAsync(false);

					if (multiScreenWindowPlacementStatus == WindowHandlerInterop.FeatureStatus.Possible)
					{
						this.aRequestMultiScreenWindowPlacementPermission = true;
						this.StateHasChanged();
					}
				}
			}
			else
			{
				await this.WindowHandler.SetMultiScreenWindowPlacementAsync(false);
			}
		}

		private void WindowManagementWindowsChanged(object? sender, EventArgs e)
		{
			_ = this.InvokeAsync(this.StateHasChanged);
		}

		private void DismissMultiScreenWindowPlacementPermission()
		{
			this.aRequestMultiScreenWindowPlacementPermission = false;
		}

		private async Task AcceptMultiScreenWindowPlacementPermission()
		{
			this.aRequestMultiScreenWindowPlacementPermission = false;
			await this.WindowHandler.SetMultiScreenWindowPlacementAsync(true);
		}
	}
}
