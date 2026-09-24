namespace KST.Blazor.Windows
{
	/// <summary>
	/// Options for KST.Blazor.Windows library
	/// </summary>
	public class BlazorWindowOptions
	{
		/// <summary>
		/// Enables the Window Management API integration used to enumerate screens and place windows
		/// across multiple displays in supported browsers.
		/// </summary>
		/// <seealso href="https://developer.mozilla.org/en-US/docs/Web/API/Window_Management_API"/>
		public bool EnableMultiScreenWindowPlacement { get; set; }
			= false;
	}
}
