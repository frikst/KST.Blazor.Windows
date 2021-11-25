using Microsoft.Extensions.DependencyInjection;

namespace KST.Blazor.Windows
{
	public static class BlazorWindowsExtensions
	{
		public static IServiceCollection AddBlazorWindows(this IServiceCollection serviceCollection)
			=> serviceCollection.AddSingleton<IWindowManagement, WindowManagementImpl>()
				.AddScoped<WindowHandlerInterop>();

	}
}
