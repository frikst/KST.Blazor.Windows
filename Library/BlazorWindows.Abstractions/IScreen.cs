namespace KST.Blazor.Windows.Abstractions
{
	/// <summary>
	/// Represents one computer screen in multi-screen or single-screen setup
	/// </summary>
	public interface IScreen
	{
		/// <summary>
		/// Left position of the screen in screen pixels
		/// </summary>
		int Left { get; }
		/// <summary>
		/// Top position of the screen in screen pixels
		/// </summary>
		int Top { get; }

		/// <summary>
		/// Width of the screen in screen pixels
		/// </summary>
		int Width { get; }
		/// <summary>
		/// Width of the screen in screen pixels
		/// </summary>
		int Height { get; }

		/// <summary>
		/// True for primary screen
		/// </summary>
		bool IsPrimary { get; }
	}
}
