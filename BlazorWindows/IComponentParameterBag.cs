using System;
using System.Linq.Expressions;

namespace KST.Blazor.Windows
{
	public interface IComponentParameterBag<TComponent>
	{
		IComponentParameterBag<TComponent> Set<TValue>(Expression<Func<TComponent, TValue>> parameter, TValue value);
	}
}
