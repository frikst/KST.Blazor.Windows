using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace KST.Blazor.Windows.Internal
{
	public partial class InternalWindowHandlerComponent : ComponentBase
	{
		private ElementReference aWindowElementRef;

		[Inject]
		public WindowHandlerInterop WindowHandler { get; set; }
			= default!;

		[Parameter]
		public IWindow Window { get; set; }
			= default!;

		public void RenderWindowContent(RenderTreeBuilder builder)
		{
			if (this.Window is WindowImpl impl)
			{
				builder.OpenComponent(0, impl.ComponentType);

				if (impl.Parameters.Any())
					builder.AddMultipleAttributes(1, impl.Parameters);

				builder.CloseComponent();
			}
		}

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			await base.OnAfterRenderAsync(firstRender);

			if (!firstRender)
				return;

			if (this.Window is WindowImpl impl)
			{
				await this.WindowHandler.OpenWindow(impl.Id, this.aWindowElementRef, impl.WindowOptions.BuildWindowFeatures(), impl.WindowOptions.Title);
				impl.AfterOpen();
			}
		}
	}
}
