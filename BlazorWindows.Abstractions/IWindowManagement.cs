using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace KST.Blazor.Windows.Abstractions
{
	/// <summary>
	/// Entry point for browser window manipulation
	/// </summary>
	public interface IWindowManagement
	{
		/// <summary>
		/// List of browser windows already opened by blazor
		/// </summary>
		IReadOnlyCollection<IWindow> Windows { get; }
		/// <summary>
		/// Event raised when window list was changed
		/// </summary>
		event EventHandler? WindowsChanged;

		/// <summary>
		/// List of screens in current user computer setup.
		/// </summary>
		IReadOnlyCollection<IScreen> Screens { get; }

		/// <summary>
		/// Open a new window containing component
		/// </summary>
		/// <typeparam name="TComponent">Type of blazor component to be shown in new window</typeparam>
		/// <returns>Reference to the newly opened window</returns>
		Task<IWindow<TComponent>> OpenWindowAsync<TComponent>()
			where TComponent : ComponentBase;

		/// <summary>
		/// Open a new window containing component
		/// </summary>
		/// <param name="parameters">Parameters to be set in the component</param>
		/// <typeparam name="TComponent">Type of blazor component to be shown in new window</typeparam>
		/// <returns>Reference to the newly opened window</returns>
		Task<IWindow<TComponent>> OpenWindowAsync<TComponent>(Action<IComponentParameterBag<TComponent>> parameters)
			where TComponent : ComponentBase;

		/// <summary>
		/// Open a new window containing component
		/// </summary>
		/// <param name="options">Initial options for a new window</param>
		/// <typeparam name="TComponent">Type of blazor component to be shown in new window</typeparam>
		/// <returns>Reference to the newly opened window</returns>
		Task<IWindow<TComponent>> OpenWindowAsync<TComponent>(NewWindowOptions options)
			where TComponent : ComponentBase;

		/// <summary>
		/// Open a new window containing component
		/// </summary>
		/// <param name="options">Initial options for a new window</param>
		/// <param name="parameters">Parameters to be set in the component</param>
		/// <typeparam name="TComponent">Type of blazor component to be shown in new window</typeparam>
		/// <returns>Reference to the newly opened window</returns>
		Task<IWindow<TComponent>> OpenWindowAsync<TComponent>(NewWindowOptions options, Action<IComponentParameterBag<TComponent>> parameters)
			where TComponent : ComponentBase;
	}
}
