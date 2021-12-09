using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace KST.Blazor.Windows
{
	public interface IComponentParameterBag<TComponent>
	{
		IComponentParameterBag<TComponent> Set<TValue>(Expression<Func<TComponent, TValue>> parameter, TValue value);

		IComponentParameterBag<TComponent> Set<TValue>(Expression<Func<TComponent, EventCallback<TValue>>> parameter, Action callback);
		IComponentParameterBag<TComponent> Set<TValue>(Expression<Func<TComponent, EventCallback<TValue>>> parameter, Action<TValue> callback);
		IComponentParameterBag<TComponent> Set<TValue>(Expression<Func<TComponent, EventCallback<TValue>>> parameter, Func<Task> callback);
		IComponentParameterBag<TComponent> Set<TValue>(Expression<Func<TComponent, EventCallback<TValue>>> parameter, Func<TValue, Task> callback);
		IComponentParameterBag<TComponent> Set(Expression<Func<TComponent, EventCallback>> parameter, Action callback);
		IComponentParameterBag<TComponent> Set(Expression<Func<TComponent, EventCallback>> parameter, Func<Task> callback);
	}
}
