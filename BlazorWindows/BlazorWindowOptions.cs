namespace KST.Blazor.Windows
{
	public class BlazorWindowOptions
	{
		/// <summary>
		/// Enables the library to span windows across multiple screens. The feature is nonstandard yet
		/// and accessible in google chrome only.
		/// </summary>
		/// <seealso href="https://chromestatus.com/feature/5252960583942144"/>
		public bool EnableMultiScreenWindowPlacement { get; set; }
			= false;
	}
}
