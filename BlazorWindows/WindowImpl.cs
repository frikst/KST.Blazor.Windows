using System;
using Microsoft.AspNetCore.Components;

namespace KST.Blazor.Windows
{
	internal abstract class WindowImpl : IWindow
	{
		protected WindowImpl()
		{
			this.Id = Guid.NewGuid();
		}

		public Guid Id { get; }

		public abstract Type ComponentType { get; }
	}

	internal class WindowImpl<TComponent> : WindowImpl, IWindow<TComponent>
		where TComponent : ComponentBase
	{
		public override Type ComponentType
			=> typeof(TComponent);
	}
}