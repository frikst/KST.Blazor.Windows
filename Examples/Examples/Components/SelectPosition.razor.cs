using System.Collections.Generic;
using System.Threading.Tasks;
using KST.Blazor.Windows.Abstractions;
using Microsoft.AspNetCore.Components;

namespace KST.Blazor.Windows.Examples.Components;

public partial class SelectPosition
{
	private int aSelectedPosition = 0;

	private static readonly IReadOnlyList<WindowPositionAtScreen> aPosiblePositions =
	[
		new WindowPositionDefault(),
		new WindowPositionAbsolute(100, 100, 200, 200),
		new WindowPositionCentered(200, 200),
		new WindowPositionMaximized()
	];

	[Parameter]
	public WindowPosition Position { get; set; }
		= aPosiblePositions[0];

	[Parameter]
	public EventCallback<WindowPosition> PositionChanged { get; set; }

	private async Task OnSelectedPositionChanged()
	{
		this.Position = aPosiblePositions[this.aSelectedPosition];
		await this.PositionChanged.InvokeAsync(this.Position);
	}
}
