namespace KST.Blazor.Windows
{
	public record NewWindowOptions
	{
		public int? Left { get; init; }
			= null;
		public int? Top { get; init; }
			= null;

		public int? Width { get; init; }
			= null;
		public int? Height { get; init; }
			= null;
	}
}
