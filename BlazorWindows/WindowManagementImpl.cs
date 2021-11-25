using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace KST.Blazor.Windows
{
	internal class WindowManagementImpl : IWindowManagement
	{
		public Task<IWindow<TComponent>> OpenWindow<TComponent>()
			where TComponent : ComponentBase
			=> this.OpenWindow<TComponent>(new NewWindowOptions());

		public Task<IWindow<TComponent>> OpenWindow<TComponent>(NewWindowOptions options)
			where TComponent : ComponentBase
			=> Task.FromException<IWindow<TComponent>>(new System.NotImplementedException());
	}
}
