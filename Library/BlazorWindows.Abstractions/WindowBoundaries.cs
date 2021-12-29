namespace KST.Blazor.Windows.Abstractions
{
	/// <summary>
	/// True Window position information
	/// </summary>
	/// <param name="Screen">Screen, that the window is located on</param>
	/// <param name="Left">Left position of the window in screen pixels</param>
	/// <param name="Top">Top position of the window in screen pixels</param>
	/// <param name="Width">Outer width of the window in screen pixels</param>
	/// <param name="Height">Outer height of the window in screen pixels</param>
	/// <param name="InnerWidth">Inner width of the window in screen pixels</param>
	/// <param name="InnerHeight">Inner height of the window in screen pixels</param>
	public record WindowBoundaries(
		IScreen Screen,
		int Left,
		int Top,
		int Width,
		int Height,
		int InnerWidth,
		int InnerHeight
	);
}
