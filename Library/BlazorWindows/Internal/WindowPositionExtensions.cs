using KST.Blazor.Windows.Abstractions;
using KST.Blazor.Windows.Internal.Interop;
using System.Diagnostics;

namespace KST.Blazor.Windows.Internal;

internal static class WindowPositionExtensions
{
	extension (WindowPosition windowPosition)
	{
		public WindowPositionInterop BuildWindowPosition(IScreen defaultScreen)
		{
			var screen = (windowPosition as WindowPositionAtScreen)?.Screen ?? defaultScreen;

			return windowPosition switch
			{
				WindowPositionAbsolute position => WindowPosition.BuildPosition(
					screen,
					position.Left,
					position.Top,
					position.Width,
					position.Height
				),
				WindowPositionCentered position => WindowPosition.BuildPosition(
					screen,
					(screen.Width - position.Width) / 2,
					(screen.Height - position.Height) / 2,
					position.Width,
					position.Height
				),
				WindowPositionDefault { Screen: null } => new WindowPositionInterop(
					WindowPositionKind.Window
				),
				WindowPositionDefault position => WindowPosition.BuildPosition(
					screen,
					0,
					0
				),
				WindowPositionMaximized position => WindowPosition.BuildPosition(
					screen,
					0,
					0,
					screen.Width,
					screen.Height,
					WindowPositionKind.Maximized
				),
				WindowPositionInTab => new WindowPositionInterop(
					WindowPositionKind.Tab
				),
				_ => throw new UnreachableException()
			};
		}

		private static WindowPositionInterop BuildPosition(IScreen screen, int left, int top, int? width = null, int? height = null, WindowPositionKind positionKind = WindowPositionKind.Window)
		{
			return new WindowPositionInterop(
				positionKind,
				left + screen.Left,
				top + screen.Top,
				width,
				height
			);
		}
	}
}
