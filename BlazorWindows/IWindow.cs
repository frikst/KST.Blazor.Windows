using Microsoft.AspNetCore.Components;

namespace KST.Blazor.Windows
{
	public interface IWindow
	{
	}

	public interface IWindow<TComponent> : IWindow
		where TComponent : ComponentBase
	{
	}
}
