using System.Linq;
using System.Threading.Tasks;
using KST.Blazor.Windows.Abstractions;
using Microsoft.AspNetCore.Components;

namespace KST.Blazor.Windows.Examples.Components;

public partial class SelectScreen
{
	private int aSelectedScreen = -1;

	[Inject]
	public required IWindowManagement WindowManagement { get; set; }

	[Parameter]
	public IScreen? Screen { get; set; }

	[Parameter]
	public EventCallback<IScreen?> ScreenChanged { get; set; }

	private async Task OnSelectedScreenChanged()
	{
		if (this.aSelectedScreen >= 0)
			this.Screen = this.WindowManagement.Screens.ElementAt(this.aSelectedScreen);
		else
			this.Screen = null;

		await this.ScreenChanged.InvokeAsync(this.Screen);
	}
}
