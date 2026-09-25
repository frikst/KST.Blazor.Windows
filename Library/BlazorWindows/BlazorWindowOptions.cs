namespace KST.Blazor.Windows
{
	/// <summary>
	/// Options for KST.Blazor.Windows library
	/// </summary>
	public class BlazorWindowOptions
	{
		/// <summary>
		/// Enables the library to use window management API. The feature is
		/// accessible only in google chrome for now.
		/// </summary>
		/// <seealso href="https://chromestatus.com/feature/5252960583942144"/>
		public bool EnableWindowManagementAPI { get; set; }
			= false;
	}
}
