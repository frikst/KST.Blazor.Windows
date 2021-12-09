using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace KST.Blazor.Windows
{
	public abstract class ComponentParameterBag
	{
		public abstract IReadOnlyDictionary<string, object?> BuildParameters();

		public abstract bool IsEmpty { get; }

		public abstract event EventHandler? Changed;
	}

	public class ComponentParameterBag<TComponent> : ComponentParameterBag, IComponentParameterBag<TComponent>
	{
		private readonly Dictionary<string, object?> aProperties;

		public ComponentParameterBag()
		{
			this.aProperties = new Dictionary<string, object?>();
		}

		public void Apply(Action<IComponentParameterBag<TComponent>> properties)
		{
			properties(this);
			this.Changed?.Invoke(this, EventArgs.Empty);
		}

		public IComponentParameterBag<TComponent> Set<TValue>(Expression<Func<TComponent, TValue>> parameter, TValue value)
		{
			this.aProperties[((MemberExpression)parameter.Body).Member.Name] = value;
			return this;
		}

		public IComponentParameterBag<TComponent> Set<TValue>(Expression<Func<TComponent, EventCallback<TValue>>> parameter, Action callback)
		{
			this.aProperties[((MemberExpression)parameter.Body).Member.Name] = new EventCallback<TValue>(callback.Target as IHandleEvent, callback);
			return this;
		}

		public IComponentParameterBag<TComponent> Set<TValue>(Expression<Func<TComponent, EventCallback<TValue>>> parameter, Action<TValue> callback)
		{
			this.aProperties[((MemberExpression)parameter.Body).Member.Name] = new EventCallback<TValue>(callback.Target as IHandleEvent, callback);
			return this;
		}

		public IComponentParameterBag<TComponent> Set<TValue>(Expression<Func<TComponent, EventCallback<TValue>>> parameter, Func<Task> callback)
		{
			this.aProperties[((MemberExpression)parameter.Body).Member.Name] = new EventCallback<TValue>(callback.Target as IHandleEvent, callback);
			return this;
		}

		public IComponentParameterBag<TComponent> Set<TValue>(Expression<Func<TComponent, EventCallback<TValue>>> parameter, Func<TValue, Task> callback)
		{
			this.aProperties[((MemberExpression)parameter.Body).Member.Name] = new EventCallback<TValue>(callback.Target as IHandleEvent, callback);
			return this;
		}

		public IComponentParameterBag<TComponent> Set(Expression<Func<TComponent, EventCallback>> parameter, Action callback)
		{
			this.aProperties[((MemberExpression)parameter.Body).Member.Name] = new EventCallback(callback.Target as IHandleEvent, callback);
			return this;
		}

		public IComponentParameterBag<TComponent> Set(Expression<Func<TComponent, EventCallback>> parameter, Func<Task> callback)
		{
			this.aProperties[((MemberExpression)parameter.Body).Member.Name] = new EventCallback(callback.Target as IHandleEvent, callback);
			return this;
		}

		public override IReadOnlyDictionary<string, object?> BuildParameters()
			=> new ReadOnlyDictionary<string, object?>(this.aProperties);

		public override bool IsEmpty
			=> !this.aProperties.Any();

		public override event EventHandler? Changed;
	}
}
