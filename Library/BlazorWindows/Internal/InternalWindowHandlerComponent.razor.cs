using System;
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
		public WindowHandlerInterop WindowHandler { get; set; }
			= default!;

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
				await this.WindowHandler.OpenWindowAsync(impl.Id, this.aWindowElementRef, this.BuildWindowPosition(impl.WindowOptions.InitialPosition), impl.WindowOptions.Title);
				impl.AfterOpen();
			}
		}

		private WindowPositionInterop BuildWindowPosition(Abstractions.WindowPosition initialPosition)
		{
			switch (initialPosition)
			{
				case WindowPositionAbsolute position:
					return BuildPosition(position.Screen, position.Left, position.Top, position.Width, position.Height);
				case WindowPositionCentered position:
					return BuildPosition(position.Screen, (position.Screen!.Width - position.Width) / 2, (position.Screen!.Height - position.Height) / 2, position.Width, position.Height);
				case WindowPositionDefault { Screen: null }:
					return new WindowPositionInterop(WindowPositionKind.Window);
				case WindowPositionDefault position:
					return BuildPosition(position.Screen, 0, 0);
				case WindowPositionMaximized position:
					return BuildPosition(position.Screen, 0, 0, position.Screen!.Width, position.Screen!.Height, WindowPositionKind.Maximized);
				case WindowPositionInTab:
					return new WindowPositionInterop(WindowPositionKind.Tab);
				default:
					throw new ArgumentOutOfRangeException(nameof(initialPosition));
			}

			static WindowPositionInterop BuildPosition(IScreen? screen, int left, int top, int? width = null, int? height = null, WindowPositionKind positionKind = WindowPositionKind.Window)
			{
				if (screen is null)
					return new WindowPositionInterop(positionKind, left, top, width, height);
				else
					return new WindowPositionInterop(positionKind, left + screen.Left, top + screen.Top, width, height);
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
