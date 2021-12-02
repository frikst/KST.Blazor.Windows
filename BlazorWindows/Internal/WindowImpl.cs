using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace KST.Blazor.Windows.Internal
{
	internal abstract class WindowImpl : IWindow
	{
		private readonly TaskCompletionSource aOpenTask;

		protected WindowImpl(NewWindowOptions options, IReadOnlyDictionary<string, object> parameters)
		{
			this.Id = Guid.NewGuid();
			this.WindowOptions = options;
			this.Parameters = parameters;
			this.aOpenTask = new TaskCompletionSource();
		}

		public Guid Id { get; }

		public abstract Type ComponentType { get; }

		public NewWindowOptions WindowOptions { get; }

		public IReadOnlyDictionary<string, object> Parameters { get; }

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
		public WindowImpl(NewWindowOptions options, IReadOnlyDictionary<string, object> parameters)
			: base(options, parameters)
		{
			
		}

		public override Type ComponentType
			=> typeof(TComponent);
	}
}
