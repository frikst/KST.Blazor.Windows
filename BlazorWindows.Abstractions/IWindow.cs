using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace KST.Blazor.Windows.Abstractions
{
	public interface IWindow
	{
		Guid Id { get; }

		Type ComponentType { get; }

		string Title { get; }

		bool IsDisposed { get; }

		Task ChangeTitle(string title);
	}

	public interface IWindow<TComponent> : IWindow
		where TComponent : ComponentBase
	{
		void ChangeParameters(Action<IComponentParameterBag<TComponent>> parameters);
	}
}
