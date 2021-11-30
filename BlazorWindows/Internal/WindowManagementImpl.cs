using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace KST.Blazor.Windows.Internal
{
	internal class WindowManagementImpl : IWindowManagement
	{
		private readonly List<WindowImpl> aWindows;

		public WindowManagementImpl()
		{
			this.aWindows = new List<WindowImpl>();
		}

		public event EventHandler? WindowsChanged;

		public IReadOnlyCollection<WindowImpl> Windows
			=> this.aWindows;

		public async Task<IWindow<TComponent>> OpenWindow<TComponent>()
			where TComponent : ComponentBase
			=> await this.OpenWindow<TComponent>(new NewWindowOptions());

		public async Task<IWindow<TComponent>> OpenWindow<TComponent>(NewWindowOptions options)
			where TComponent : ComponentBase
		{
			var newWindow = new WindowImpl<TComponent>(options);
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
