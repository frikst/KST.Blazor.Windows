using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace KST.Blazor.Windows.Abstractions
{
	public interface IWindowManagement
	{
		IReadOnlyCollection<IWindow> Windows { get; }
		event EventHandler? WindowsChanged;

		IReadOnlyCollection<IScreen> Screens { get; }

		Task<IWindow<TComponent>> OpenWindow<TComponent>()
			where TComponent : ComponentBase;

		Task<IWindow<TComponent>> OpenWindow<TComponent>(Action<IComponentParameterBag<TComponent>> parameters)
			where TComponent : ComponentBase;

		Task<IWindow<TComponent>> OpenWindow<TComponent>(NewWindowOptions options)
			where TComponent : ComponentBase;

		Task<IWindow<TComponent>> OpenWindow<TComponent>(NewWindowOptions options, Action<IComponentParameterBag<TComponent>> parameters)
			where TComponent : ComponentBase;
	}
}
