using System;
using System.Threading.Tasks;
using KST.Blazor.Windows.Abstractions;
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
				await this.WindowHandler.OpenWindowAsync(impl.Id, this.aWindowElementRef, this.BuildWindowFeatures(impl.WindowOptions.InitialPosition), impl.WindowOptions.Title);
				impl.AfterOpen();
			}
		}

		private string BuildWindowFeatures(WindowPosition initialPosition)
		{
			switch (initialPosition)
			{
				case WindowPositionAbsolute position:
					return $"popup=yes, {BuildPositionWithSize(position.Screen, position.Left, position.Top, position.Width, position.Height)}";
				case WindowPositionDefault { Screen: null }:
					return "popup=yes";
				case WindowPositionDefault position:
					return $"popup=yes, {BuildPosition(position.Screen, 0, 0)}";
				case WindowPositionInTab:
					return string.Empty;
				default:
					throw new ArgumentOutOfRangeException(nameof(initialPosition));
			}

			static string BuildPositionWithSize(IScreen? screen, int left, int top, int width, int height)
			{
				if (screen is null)
					return $"left={left}, top={top}, width={width}, height={height}";
				else
					return $"left={left + screen.Left}, top={top + screen.Top}, width={width}, height={height}";
			}

			static string BuildPosition(IScreen? screen, int left, int top)
			{
				if (screen is null)
					return $"left={left}, top={top}";
				else
					return $"left={left + screen.Left}, top={top + screen.Top}";
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
