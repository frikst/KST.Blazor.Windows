using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using KST.Blazor.Windows.Abstractions;
using Microsoft.AspNetCore.Components;

namespace KST.Blazor.Windows
{
	/// <summary>
	/// Container for component parameters
	/// </summary>
	public abstract class ComponentParameterBag
	{
		/// <summary>
		/// Creates dictionary with all set parameters
		/// </summary>
		/// <returns>The parameter dictionary</returns>
		public abstract IReadOnlyDictionary<string, object?> BuildParameters();

		/// <summary>
		/// True if there was no parameters set
		/// </summary>
		public abstract bool IsEmpty { get; }

		/// <summary>
		/// Raised every time parameters are changed
		/// </summary>
		public abstract event EventHandler? Changed;
	}

	/// <inheritdoc cref="IComponentParameterBag{TComponent}"/>
	public class ComponentParameterBag<TComponent> : ComponentParameterBag, IComponentParameterBag<TComponent>
	{
		private readonly Dictionary<string, object?> aProperties;

		/// <summary>
		/// Initializes a new empty parameter bag
		/// </summary>
		public ComponentParameterBag()
		{
			this.aProperties = new Dictionary<string, object?>();
		}

		/// <summary>
		/// Apply parameter changes
		/// </summary>
		/// <param name="parameters">New parameter values</param>
		public void Apply(Action<IComponentParameterBag<TComponent>> parameters)
		{
			parameters(this);
			this.Changed?.Invoke(this, EventArgs.Empty);
		}

		/// <inheritdoc />
		public IComponentParameterBag<TComponent> Set<TValue>(Expression<Func<TComponent, TValue>> parameter, TValue value)
		{
			this.aProperties[((MemberExpression)parameter.Body).Member.Name] = value;
			return this;
		}

		/// <inheritdoc />
		public IComponentParameterBag<TComponent> Set<TValue>(Expression<Func<TComponent, EventCallback<TValue>>> parameter, Action callback)
		{
			this.aProperties[((MemberExpression)parameter.Body).Member.Name] = new EventCallback<TValue>(callback.Target as IHandleEvent, callback);
			return this;
		}

		/// <inheritdoc />
		public IComponentParameterBag<TComponent> Set<TValue>(Expression<Func<TComponent, EventCallback<TValue>>> parameter, Action<TValue> callback)
		{
			this.aProperties[((MemberExpression)parameter.Body).Member.Name] = new EventCallback<TValue>(callback.Target as IHandleEvent, callback);
			return this;
		}

		/// <inheritdoc />
		public IComponentParameterBag<TComponent> Set<TValue>(Expression<Func<TComponent, EventCallback<TValue>>> parameter, Func<Task> callback)
		{
			this.aProperties[((MemberExpression)parameter.Body).Member.Name] = new EventCallback<TValue>(callback.Target as IHandleEvent, callback);
			return this;
		}

		/// <inheritdoc />
		public IComponentParameterBag<TComponent> Set<TValue>(Expression<Func<TComponent, EventCallback<TValue>>> parameter, Func<TValue, Task> callback)
		{
			this.aProperties[((MemberExpression)parameter.Body).Member.Name] = new EventCallback<TValue>(callback.Target as IHandleEvent, callback);
			return this;
		}

		/// <inheritdoc />
		public IComponentParameterBag<TComponent> Set(Expression<Func<TComponent, EventCallback>> parameter, Action callback)
		{
			this.aProperties[((MemberExpression)parameter.Body).Member.Name] = new EventCallback(callback.Target as IHandleEvent, callback);
			return this;
		}

		/// <inheritdoc />
		public IComponentParameterBag<TComponent> Set(Expression<Func<TComponent, EventCallback>> parameter, Func<Task> callback)
		{
			this.aProperties[((MemberExpression)parameter.Body).Member.Name] = new EventCallback(callback.Target as IHandleEvent, callback);
			return this;
		}

		/// <inheritdoc />
		public override IReadOnlyDictionary<string, object?> BuildParameters()
			=> new ReadOnlyDictionary<string, object?>(this.aProperties);

		/// <inheritdoc />
		public override bool IsEmpty
			=> !this.aProperties.Any();

		/// <inheritdoc />
		public override event EventHandler? Changed;
	}
}
