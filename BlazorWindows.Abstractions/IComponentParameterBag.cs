using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace KST.Blazor.Windows.Abstractions
{
	/// <summary>
	/// Container for component parameters
	/// </summary>
	/// <typeparam name="TComponent">Type of the component</typeparam>
	public interface IComponentParameterBag<TComponent>
	{
		/// <summary>
		/// Set parameter value
		/// </summary>
		/// <param name="parameter">Reference to the parameter property</param>
		/// <param name="value">New value to be set to the parameter</param>
		/// <typeparam name="TValue">Type of the parameter</typeparam>
		/// <returns>A reference to this instance after the operation has completed</returns>
		IComponentParameterBag<TComponent> Set<TValue>(Expression<Func<TComponent, TValue>> parameter, TValue value);

		/// <summary>
		/// Set callback parameter value
		/// </summary>
		/// <param name="parameter">Reference to the parameter property</param>
		/// <param name="callback">New callback to be set ti the parameter</param>
		/// <typeparam name="TValue">Type of the callback argument</typeparam>
		/// <returns>A reference to this instance after the operation has completed</returns>
		IComponentParameterBag<TComponent> Set<TValue>(Expression<Func<TComponent, EventCallback<TValue>>> parameter, Action callback);

		/// <summary>
		/// Set callback parameter value
		/// </summary>
		/// <param name="parameter">Reference to the parameter property</param>
		/// <param name="callback">New callback to be set ti the parameter</param>
		/// <typeparam name="TValue">Type of the callback argument</typeparam>
		/// <returns>A reference to this instance after the operation has completed</returns>
		IComponentParameterBag<TComponent> Set<TValue>(Expression<Func<TComponent, EventCallback<TValue>>> parameter, Action<TValue> callback);

		/// <summary>
		/// Set callback parameter value
		/// </summary>
		/// <param name="parameter">Reference to the parameter property</param>
		/// <param name="callback">New callback to be set ti the parameter</param>
		/// <typeparam name="TValue">Type of the callback argument</typeparam>
		/// <returns>A reference to this instance after the operation has completed</returns>
		IComponentParameterBag<TComponent> Set<TValue>(Expression<Func<TComponent, EventCallback<TValue>>> parameter, Func<Task> callback);

		/// <summary>
		/// Set callback parameter value
		/// </summary>
		/// <param name="parameter">Reference to the parameter property</param>
		/// <param name="callback">New callback to be set ti the parameter</param>
		/// <typeparam name="TValue">Type of the callback argument</typeparam>
		/// <returns>A reference to this instance after the operation has completed</returns>
		IComponentParameterBag<TComponent> Set<TValue>(Expression<Func<TComponent, EventCallback<TValue>>> parameter, Func<TValue, Task> callback);

		/// <summary>
		/// Set callback parameter value
		/// </summary>
		/// <param name="parameter">Reference to the parameter property</param>
		/// <param name="callback">New callback to be set ti the parameter</param>
		/// <returns>A reference to this instance after the operation has completed</returns>
		IComponentParameterBag<TComponent> Set(Expression<Func<TComponent, EventCallback>> parameter, Action callback);

		/// <summary>
		/// Set callback parameter value
		/// </summary>
		/// <param name="parameter">Reference to the parameter property</param>
		/// <param name="callback">New callback to be set ti the parameter</param>
		/// <returns>A reference to this instance after the operation has completed</returns>
		IComponentParameterBag<TComponent> Set(Expression<Func<TComponent, EventCallback>> parameter, Func<Task> callback);
	}
}
