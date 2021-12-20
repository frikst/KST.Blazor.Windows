namespace KST.Blazor.Windows.Abstractions
{
	/// <summary>
	/// Initial options for a new window
	/// </summary>
	public record NewWindowOptions
	{
		/// <summary>
		/// Initial position of a new window
		/// </summary>
		public WindowPosition InitialPosition { get; init; }
			= new WindowPositionDefault();

		/// <summary>
		/// New window title
		/// </summary>
		public string? Title { get; init; }
			= null;
	}
}
