using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace KST.Blazor.Windows
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

		public Task<IWindow<TComponent>> OpenWindow<TComponent>(NewWindowOptions options)
			where TComponent : ComponentBase
		{
			var newWindow = new WindowImpl<TComponent>();
			this.aWindows.Add(newWindow);
			this.WindowsChanged?.Invoke(this, EventArgs.Empty);
			return Task.FromResult<IWindow<TComponent>>(newWindow);
		}
	}
}
