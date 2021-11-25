using Microsoft.AspNetCore.Components;

namespace KST.Blazor.Windows
{
	public interface IWindowManagement
	{
		IWindow<TComponent> OpenWindow<TComponent>(NewWindowOptions options)
			where TComponent : ComponentBase;
	}
}
