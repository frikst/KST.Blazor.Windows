using System;
using Microsoft.AspNetCore.Components;

namespace KST.Blazor.Windows
{
	public partial class WindowContainer
	{
		[Inject]
		public IWindowManagement WindowManagement { get; set; }
			= default!;
	}
}
