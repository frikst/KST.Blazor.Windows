using System;
using System.Threading.Tasks;
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

		public async Task ChangeTitle(string title)
		{
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
			((ComponentParameterBag<TComponent>)this.Parameters).Apply(parameters);
		}
	}
}
