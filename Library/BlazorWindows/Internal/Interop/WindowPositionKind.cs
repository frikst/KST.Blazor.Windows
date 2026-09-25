using System.Text.Json.Serialization;

namespace KST.Blazor.Windows.Internal.Interop;

/// <summary>
/// The kind of window position.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum WindowPositionKind
{
	/// <summary>
	/// The window is positioned in a new browser window.
	/// </summary>
	Window,
	/// <summary>
	/// The window is positioned in a browser tab.
	/// </summary>
	Tab,
	/// <summary>
	/// The window is displayed maximized.
	/// </summary>
	Maximized
}