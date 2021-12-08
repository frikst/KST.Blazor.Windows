using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace KST.Blazor.Windows.Internal
{
	internal class WindowManagementImpl : IWindowManagement
	{
		private readonly WindowHandlerInterop aWindowHandler;
		private readonly List<WindowImpl> aWindows;

		public WindowManagementImpl(WindowHandlerInterop windowHandler)
		{
			this.aWindowHandler = windowHandler;

			this.aWindows = new List<WindowImpl>();
		}

		public event EventHandler? WindowsChanged;

		public IReadOnlyCollection<WindowImpl> Windows
			=> this.aWindows;

		public async Task<IWindow<TComponent>> OpenWindow<TComponent>()
			where TComponent : ComponentBase
			=> await this.OpenWindow<TComponent>(NewWindowOptions.Empty, null!);

		public async Task<IWindow<TComponent>> OpenWindow<TComponent>(Action<IComponentParameterBag<TComponent>> parameters)
			where TComponent : ComponentBase
			=> await this.OpenWindow<TComponent>(NewWindowOptions.Empty, parameters);

		public async Task<IWindow<TComponent>> OpenWindow<TComponent>(NewWindowOptions options)
			where TComponent : ComponentBase
			=> await this.OpenWindow<TComponent>(options, null!);

		public async Task<IWindow<TComponent>> OpenWindow<TComponent>(NewWindowOptions options, Action<IComponentParameterBag<TComponent>> parameters)
			where TComponent : ComponentBase
		{
			var parameterBag = new ComponentParameterBag<TComponent>();

			if (parameters is not null)
				parameterBag.Apply(parameters);

			var newWindow = new WindowImpl<TComponent>(this.aWindowHandler, options, parameterBag);
			this.aWindows.Add(newWindow);
			this.WindowsChanged?.Invoke(this, EventArgs.Empty);
			await newWindow.WaitOpen();
			return newWindow;
		}

		public void OnWindowClosed(Guid id)
		{
			this.aWindows.RemoveAll(x => x.Id == id);
			this.WindowsChanged?.Invoke(this, EventArgs.Empty);
		}
	}
}
