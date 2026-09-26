using KST.Blazor.Windows.Abstractions;
using Microsoft.AspNetCore.Components;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace KST.Blazor.Windows.Examples.Components;

public partial class SelectWindow : IDisposable
{
	private int aSelectedWindow = -1;

	[Inject]
	public required IWindowManagement WindowManagement { get; set; }

	[Parameter]
	public IWindow? Window { get; set; }

	[Parameter]
	public EventCallback<IWindow?> WindowChanged { get; set; }

	protected override void OnInitialized()
	{
		base.OnInitialized();

		this.WindowManagement.WindowsChanged += this.OnWindowsChanged;
	}

	private void OnWindowsChanged(object? sender, EventArgs e)
	{
		this.StateHasChanged();
	}

	public void Dispose()
	{
		this.WindowManagement.WindowsChanged -= this.OnWindowsChanged;
	}

	private async Task OnSelectedWindowChanged()
	{
		if (this.aSelectedWindow == -1)
			this.Window = null;
		else
			this.Window = this.WindowManagement.Windows.ElementAt(this.aSelectedWindow);
		await this.WindowChanged.InvokeAsync(this.Window);
	}
}
