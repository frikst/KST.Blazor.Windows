using KST.Blazor.Windows.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace KST.Blazor.Windows
{
	public static class BlazorWindowsExtensions
	{
		public static IServiceCollection AddBlazorWindows(this IServiceCollection serviceCollection)
			=> serviceCollection.AddScoped<IWindowManagement, WindowManagementImpl>()
				.AddScoped<WindowHandlerInterop>();

	}
}
