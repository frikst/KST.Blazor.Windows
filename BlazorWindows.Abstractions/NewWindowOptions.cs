using System.Text;

namespace KST.Blazor.Windows.Abstractions
{
	public record NewWindowOptions
	{
		public static readonly NewWindowOptions AsTab
			= new NewWindowOptions { aIsTab = true };

		public static readonly NewWindowOptions Empty
			= new NewWindowOptions();

		private bool aIsTab = false;

		public int? Left { get; init; }
			= null;
		public int? Top { get; init; }
			= null;

		public int? Width { get; init; }
			= null;
		public int? Height { get; init; }
			= null;

		public string? Title { get; init; }
			= null;

		public string BuildWindowFeatures()
		{
			if (this.aIsTab)
				return string.Empty;

			var builder = new StringBuilder("popup=yes");

			if (this.Left is int left)
				builder.Append($", left={left}");

			if (this.Top is int top)
				builder.Append($", top={top}");

			if (this.Width is int width)
				builder.Append($", width={width}");

			if (this.Height is int height)
				builder.Append($", height={height}");

			return builder.ToString();
		}
	}
}
