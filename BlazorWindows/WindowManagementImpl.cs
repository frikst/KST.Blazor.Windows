using Microsoft.AspNetCore.Components;

namespace KST.Blazor.Windows
{
	internal class WindowManagementImpl : IWindowManagement
	{
		public IWindow<TComponent> OpenWindow<TComponent>(NewWindowOptions options)
			where TComponent : ComponentBase
			=> throw new System.NotImplementedException();
	}
}
