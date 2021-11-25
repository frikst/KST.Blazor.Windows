using System;
using Microsoft.AspNetCore.Components;

namespace KST.Blazor.Windows
{
	public interface IWindow
	{
		public Guid Id { get; }
	}

	public interface IWindow<TComponent> : IWindow
		where TComponent : ComponentBase
	{
	}
}
