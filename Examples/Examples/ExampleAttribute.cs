using System;

namespace KST.Blazor.Windows.Examples
{
	public class ExampleAttribute : Attribute
	{
		public ExampleAttribute(string title)
		{
			this.Title = title;
		}

		public string Title { get; }
	}
}
