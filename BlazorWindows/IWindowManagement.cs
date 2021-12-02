using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace KST.Blazor.Windows
{
	public interface IWindowManagement
	{
		Task<IWindow<TComponent>> OpenWindow<TComponent>()
			where TComponent : ComponentBase;

		Task<IWindow<TComponent>> OpenWindow<TComponent>(IReadOnlyDictionary<string, object> parameters)
			where TComponent : ComponentBase;

		Task<IWindow<TComponent>> OpenWindow<TComponent>(NewWindowOptions options)
			where TComponent : ComponentBase;

		Task<IWindow<TComponent>> OpenWindow<TComponent>(NewWindowOptions options, IReadOnlyDictionary<string, object> parameters)
			where TComponent : ComponentBase;
	}
}
