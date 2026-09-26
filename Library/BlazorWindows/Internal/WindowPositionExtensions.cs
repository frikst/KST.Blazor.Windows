using KST.Blazor.Windows.Abstractions;
using KST.Blazor.Windows.Internal.Interop;
using System.Diagnostics;

namespace KST.Blazor.Windows.Internal;

internal static class WindowPositionExtensions
{
	extension (WindowPosition windowPosition)
	{
		public WindowPositionInterop BuildWindowPosition()
		{
			return windowPosition switch
			{
				WindowPositionAbsolute position => WindowPosition.BuildPosition(
					position.Screen,
					position.Left,
					position.Top,
					position.Width,
					position.Height
				),
				WindowPositionCentered position => WindowPosition.BuildPosition(
					position.Screen,
					(position.Screen!.Width - position.Width) / 2,
					(position.Screen!.Height - position.Height) / 2,
					position.Width,
					position.Height
				),
				WindowPositionDefault { Screen: null } => new WindowPositionInterop(
					WindowPositionKind.Window
				),
				WindowPositionDefault position => WindowPosition.BuildPosition(
					position.Screen,
					0,
					0
				),
				WindowPositionMaximized position => WindowPosition.BuildPosition(
					position.Screen,
					0,
					0,
					position.Screen!.Width,
					position.Screen!.Height,
					WindowPositionKind.Maximized
				),
				WindowPositionInTab => new WindowPositionInterop(
					WindowPositionKind.Tab
				),
				_ => throw new UnreachableException()
			};
		}

		private static WindowPositionInterop BuildPosition(IScreen? screen, int left, int top, int? width = null, int? height = null, WindowPositionKind positionKind = WindowPositionKind.Window)
		{
			return new WindowPositionInterop(
				positionKind,
				left + screen?.Left ?? 0,
				top + screen?.Top ?? 0,
				width,
				height
			);
		}
	}
}
