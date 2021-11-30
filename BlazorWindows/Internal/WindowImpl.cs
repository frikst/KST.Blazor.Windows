using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace KST.Blazor.Windows.Internal
{
	internal abstract class WindowImpl : IWindow
	{
		private readonly TaskCompletionSource aOpenTask;

		protected WindowImpl()
		{
			this.Id = Guid.NewGuid();
			this.aOpenTask = new TaskCompletionSource();
		}

		public Guid Id { get; }

		public abstract Type ComponentType { get; }

		public abstract NewWindowOptions WindowOptions { get; }

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
		public WindowImpl(NewWindowOptions options)
		{
			this.WindowOptions = options;
		}

		public override Type ComponentType
			=> typeof(TComponent);

		public override NewWindowOptions WindowOptions { get; }
	}
}
