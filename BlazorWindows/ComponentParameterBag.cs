using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;

namespace KST.Blazor.Windows
{
	public abstract class ComponentParameterBag
	{
		public abstract IReadOnlyDictionary<string, object?> BuildParameters();

		public abstract bool IsEmpty { get; }
	}

	public class ComponentParameterBag<TComponent> : ComponentParameterBag, IComponentParameterBag<TComponent>
	{
		private readonly Dictionary<string, object?> aProperties;

		public ComponentParameterBag()
		{
			this.aProperties = new Dictionary<string, object?>();
		}

		public void Apply(Action<ComponentParameterBag<TComponent>> properties)
		{
			properties(this);
		}

		public IComponentParameterBag<TComponent> Set<TValue>(Expression<Func<TComponent, TValue>> parameter, TValue value)
		{
			this.aProperties[((MemberExpression)parameter.Body).Member.Name] = value;
			return this;
		}

		public override IReadOnlyDictionary<string, object?> BuildParameters()
			=> new ReadOnlyDictionary<string, object?>(this.aProperties);

		public override bool IsEmpty
			=> !this.aProperties.Any();
	}
}
