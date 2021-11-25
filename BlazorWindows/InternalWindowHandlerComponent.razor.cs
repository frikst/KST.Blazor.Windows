using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace KST.Blazor.Windows
{
	public partial class InternalWindowHandlerComponent : ComponentBase
	{
		private ElementReference aWindowRef;

		[Parameter]
		public IWindow Window { get; set; }
			= default!;

		public void RenderWindowContent(RenderTreeBuilder builder)
		{
			if (this.Window is WindowImpl impl)
			{
				builder.OpenComponent(0, impl.ComponentType);
				builder.CloseComponent();
			}
		}
	}
}
