using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KST.Blazor.Windows.Abstractions;
using Microsoft.AspNetCore.Components;

namespace KST.Blazor.Windows.Internal
{
	internal class WindowManagementImpl : IWindowManagement
	{
		private readonly WindowHandlerInterop aWindowHandler;
		private readonly Dictionary<Guid, WindowImpl> aWindows;

		public WindowManagementImpl(WindowHandlerInterop windowHandler)
		{
			this.aWindowHandler = windowHandler;

			this.aWindows = new Dictionary<Guid, WindowImpl>();

			this.Screens = Array.Empty<IScreen>();
		}

		public event EventHandler? WindowsChanged;

		public IReadOnlyCollection<WindowImpl> Windows
			=> this.aWindows.Values;

		IReadOnlyCollection<IWindow> IWindowManagement.Windows
			=> this.aWindows.Values;

		public IReadOnlyCollection<IScreen> Screens { get; private set; }

		public async Task<IWindow<TComponent>> OpenWindow<TComponent>()
			where TComponent : ComponentBase
			=> await this.OpenWindow<TComponent>(new NewWindowOptions(), null!);

		public async Task<IWindow<TComponent>> OpenWindow<TComponent>(Action<IComponentParameterBag<TComponent>> parameters)
			where TComponent : ComponentBase
			=> await this.OpenWindow<TComponent>(new NewWindowOptions(), parameters);

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
			this.aWindows.Add(newWindow.Id, newWindow);
			this.WindowsChanged?.Invoke(this, EventArgs.Empty);
			await newWindow.WaitOpen();
			return newWindow;
		}

		public void OnWindowClosed(Guid id)
		{
			var window = this.aWindows[id];
			window.OnWindowClosed();
			this.aWindows.Remove(id);
			this.WindowsChanged?.Invoke(this, EventArgs.Empty);
		}

		public void OnScreensChanged(IEnumerable<ScreenImpl> screens)
		{
			this.Screens = screens.ToArray();
		}
	}
}
