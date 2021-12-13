using System;

namespace KST.Blazor.Windows.Abstractions
{
	public record NewWindowOptions
	{
		public static readonly NewWindowOptions Default
			= new NewWindowOptions();

		public WindowPosition InitialPosition { get; init; }
			= new WindowPositionDefault();

		public string? Title { get; init; }
			= null;
	}
}
