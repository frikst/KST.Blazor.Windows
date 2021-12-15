using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace KST.Blazor.Windows.Abstractions
{
	/// <summary>
	/// Represents reference to a browser window
	/// </summary>
	public interface IWindow
	{
		/// <summary>
		/// Browser window identifier
		/// </summary>
		Guid Id { get; }

		/// <summary>
		/// Type of the blazor component shown in the window
		/// </summary>
		Type ComponentType { get; }

		/// <summary>
		/// Window title
		/// </summary>
		string Title { get; }

		/// <summary>
		/// Changes window title to the given one.
		/// </summary>
		/// <param name="title">Required window title</param>
		Task ChangeTitle(string title);

		/// <summary>
		/// True if browser window was disposed and cannot be manipulated by user nor by Blazor application.
		/// </summary>
		bool IsDisposed { get; }
	}

	/// <summary>
	/// Represents reference to a browser window
	/// </summary>
	/// <typeparam name="TComponent">Type of the blazor component shown in the window</typeparam>
	public interface IWindow<TComponent> : IWindow
		where TComponent : ComponentBase
	{
		/// <summary>
		/// Change parameters for the contained blazor component
		/// </summary>
		/// <param name="parameters">New parameters</param>
		void ChangeParameters(Action<IComponentParameterBag<TComponent>> parameters);
	}
}
