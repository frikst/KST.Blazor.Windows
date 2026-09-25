namespace KST.Blazor.Windows.Internal.Interop
{
	/// <summary>
	/// Information about window to be opened
	/// </summary>
	/// <param name="PositionKind">Kind of the position</param>
	/// <param name="Left">Left position of window, or null if not important</param>
	/// <param name="Top">Top position of window, or null if not important</param>
	/// <param name="Width">Outer width of window, or null if not important</param>
	/// <param name="Height">Outer height of window, or null if not important</param>
	public record WindowPositionInterop(
		WindowPositionKind PositionKind,
		int? Left = null,
		int? Top = null,
		int? Width = null,
		int? Height = null
	);
}
