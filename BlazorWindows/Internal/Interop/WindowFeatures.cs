namespace KST.Blazor.Windows.Internal.Interop
{
	/// <summary>
	/// Information about window to be opened
	/// </summary>
	/// <param name="Popup">True, of window should be opened as a popup</param>
	/// <param name="Left">Left position of window, or null if not important</param>
	/// <param name="Top">Top position of window, or null if not important</param>
	/// <param name="Width">Outer width of window, or null if not important</param>
	/// <param name="Height">Outer height of window, or null if not important</param>
	public record WindowFeatures(
		bool Popup,
		int? Left = null,
		int? Top = null,
		int? Width = null,
		int? Height = null
	);
}
