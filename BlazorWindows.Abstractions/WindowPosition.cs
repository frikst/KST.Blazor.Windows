namespace KST.Blazor.Windows.Abstractions
{
	/// <summary>
	/// Window position
	/// </summary>
	public abstract record WindowPosition;

	/// <summary>
	/// Window position with screen specification
	/// </summary>
	public abstract record WindowPositionAtScreen : WindowPosition
	{
		/// <summary>
		/// Required screen or null if the screen does not matter.
		/// </summary>
		public IScreen? Screen { get; init; }
	}

	/// <summary>
	/// Window positioned in a browser tab
	/// </summary>
	public record WindowPositionInTab : WindowPosition;

	/// <summary>
	/// Default window position
	/// </summary>
	public record WindowPositionDefault : WindowPositionAtScreen;

	/// <summary>
	/// Window positioned at absolute coordinates
	/// </summary>
	/// <param name="Left">Left position of the window in screen pixels</param>
	/// <param name="Top">Top position of the window in screen pixels</param>
	/// <param name="Width">Width of the window in screen pixels</param>
	/// <param name="Height">Height of the window in screen pixels</param>
	public record WindowPositionAbsolute(int Left, int Top, int Width, int Height) : WindowPositionAtScreen;

	/// <summary>
	/// Window centered at screen
	/// </summary>
	/// <param name="Width">Width of the window in screen pixels</param>
	/// <param name="Height">Height of the window in screen pixels</param>
	public record WindowPositionCentered(int Width, int Height) : WindowPositionAtScreen;
}
