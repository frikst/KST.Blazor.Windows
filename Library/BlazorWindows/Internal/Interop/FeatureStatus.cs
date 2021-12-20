using System.Text.Json.Serialization;

namespace KST.Blazor.Windows.Internal.Interop
{
	/// <summary>
	/// Information about API availability
	/// </summary>
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public enum FeatureStatus
	{
		/// <summary>
		/// An API is not available in the current configuration
		/// </summary>
		NotPossible,
		/// <summary>
		/// An API is available in the current configuration, but not allowed by user yet
		/// </summary>
		Possible,
		/// <summary>
		/// An API is available in the current configuration
		/// </summary>
		Allowed
	}
}
