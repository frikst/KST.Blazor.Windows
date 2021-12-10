namespace KST.Blazor.Windows.Abstractions
{
	public abstract record WindowPosition;

	public abstract record WindowPositionAtScreen : WindowPosition
	{
		public IScreen? Screen { get; init; }
	}

	public record WindowPositionInTab : WindowPosition;

	public record WindowPositionDefault : WindowPositionAtScreen;

	public record WindowPositionAbsolute(int Left, int Top, int Width, int Height) : WindowPositionAtScreen;

	public record WindowPositionCentered(int Width, int Height) : WindowPositionAtScreen;
}
