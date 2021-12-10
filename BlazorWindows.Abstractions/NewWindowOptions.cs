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

		public string BuildWindowFeatures()
		{
			switch (this.InitialPosition)
			{
				case WindowPositionAbsolute position:
					return $"popup=yes, {this.BuildPosition(position.Screen, position.Left, position.Top, position.Width, position.Height)}";
				case WindowPositionDefault { Screen: null }:
					return "popup=yes";
				case WindowPositionDefault position:
					return $"popup=yes, {this.BuildPosition(position.Screen, 0, 0)}";
				case WindowPositionInTab:
					return string.Empty;
				default:
					throw new ArgumentOutOfRangeException(nameof(this.InitialPosition));
			}
		}

		private string BuildPosition(IScreen? screen, int left, int top, int width, int height)
		{
			if (screen is null)
				return $"left={left}, top={top}, width={width}, height={height}";
			else
				return $"left={left + screen.Left}, top={top + screen.Top}, width={width}, height={height}";
		}

		private string BuildPosition(IScreen? screen, int left, int top)
		{
			if (screen is null)
				return $"left={left}, top={top}";
			else
				return $"left={left + screen.Left}, top={top + screen.Top}";
		}
	}
}
