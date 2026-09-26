using System;
using System.Linq;
using System.Threading.Tasks;
using KST.Blazor.Windows.Abstractions;
using KST.Blazor.Windows.Internal.Interop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace KST.Blazor.Windows.Internal
{
	/// <summary>
	/// Component representing one window in a render tree.
	/// </summary>
	/// <remarks>For library internal use only. Should not be instantiated anywhere</remarks>
	public partial class InternalWindowHandlerComponent : ComponentBase, IDisposable
	{
		private ElementReference aWindowElementRef;
		private IWindow aWindow = default!;

		/// <summary>
		/// Javascript interop service
		/// </summary>
		[Inject]
		public required WindowHandlerInterop WindowHandler { get; set; }

		/// <summary>
		/// Window management service
		/// </summary>
		[Inject]
		public required IWindowManagement WindowManagement { get; set; }

		/// <summary>
		/// Window to be managed by the component
		/// </summary>
		[Parameter]
#pragma warning disable BL0007 // Component parameters should be auto properties
		public IWindow Window
		{
			get => this.aWindow;
			set
			{
				if (this.aWindow is WindowImpl oldImpl)
					oldImpl.Parameters.Changed -= this.OnWindowParametersChanged;

				this.aWindow = value;

				if (this.aWindow is WindowImpl newImpl)
					newImpl.Parameters.Changed += this.OnWindowParametersChanged;
			}
		}
#pragma warning restore BL0007 // Component parameters should be auto properties

		private void OnWindowParametersChanged(object? sender, EventArgs e)
		{
			this.StateHasChanged();
		}

		private void RenderWindowContent(RenderTreeBuilder builder)
		{
			if (this.Window is WindowImpl impl)
			{
				builder.OpenComponent(0, impl.ComponentType);

				if (!impl.Parameters.IsEmpty)
					builder.AddMultipleAttributes(1, impl.Parameters.BuildParameters()!);

				builder.CloseComponent();
			}
		}

		/// <inheritdoc />
		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			await base.OnAfterRenderAsync(firstRender);

			if (!firstRender)
				return;

			if (this.Window is WindowImpl impl)
			{
				await this.WindowHandler.OpenWindowAsync(
					impl.Id,
					this.aWindowElementRef,
					impl.WindowOptions.InitialPosition.BuildWindowPosition(
						this.WindowManagement.Screens.First()
					),
					impl.WindowOptions.Title
				);
				impl.AfterOpen();
			}
		}

		/// <inheritdoc />
		public void Dispose()
		{
			if (this.aWindow is WindowImpl oldImpl)
				oldImpl.Parameters.Changed -= this.OnWindowParametersChanged;
		}
	}
}
