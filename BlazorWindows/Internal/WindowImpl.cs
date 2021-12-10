using System;
using System.Threading.Tasks;
using KST.Blazor.Windows.Abstractions;
using Microsoft.AspNetCore.Components;

namespace KST.Blazor.Windows.Internal
{
	internal abstract class WindowImpl : IWindow
	{
		private readonly WindowHandlerInterop aWindowHandler;
		private readonly TaskCompletionSource aOpenTask;

		protected WindowImpl(WindowHandlerInterop windowHandler, NewWindowOptions options, ComponentParameterBag parameters)
		{
			this.aWindowHandler = windowHandler;
			this.Id = Guid.NewGuid();
			this.WindowOptions = options;
			this.Parameters = parameters;
			this.Title = options.Title ?? string.Empty;
			this.aOpenTask = new TaskCompletionSource();
		}

		public Guid Id { get; }

		public string Title { get; private set; }

		public bool IsDisposed { get; private set; }

		public async Task ChangeTitle(string title)
		{
			if (this.IsDisposed)
				throw new InvalidOperationException("Cannot modify title of disposed window");

			await this.aWindowHandler.ChangeWindowTitle(this.Id, title);
			this.Title = title;
		}

		public abstract Type ComponentType { get; }

		public NewWindowOptions WindowOptions { get; }

		public ComponentParameterBag Parameters { get; }

		public Task WaitOpen()
			=> this.aOpenTask.Task;

		public void AfterOpen()
		{
			this.aOpenTask.SetResult();
		}

		public void OnWindowClosed()
		{
			this.IsDisposed = true;
		}
	}

	internal class WindowImpl<TComponent> : WindowImpl, IWindow<TComponent>
		where TComponent : ComponentBase
	{
		public WindowImpl(WindowHandlerInterop windowHandler, NewWindowOptions options, ComponentParameterBag<TComponent> parameterBag)
			: base(windowHandler, options, parameterBag)
		{
			
		}

		public override Type ComponentType
			=> typeof(TComponent);

		public void ChangeParameters(Action<IComponentParameterBag<TComponent>> parameters)
		{
			if (this.IsDisposed)
				throw new InvalidOperationException("Cannot modify parameters of component in disposed window");

			((ComponentParameterBag<TComponent>)this.Parameters).Apply(parameters);
		}
	}
}
