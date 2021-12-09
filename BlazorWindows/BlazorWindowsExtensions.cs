using System;
using KST.Blazor.Windows.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace KST.Blazor.Windows
{
	public static class BlazorWindowsExtensions
	{
		public static IServiceCollection AddBlazorWindows(this IServiceCollection serviceCollection)
			=> serviceCollection.AddScoped<IWindowManagement, WindowManagementImpl>()
				.AddScoped<WindowHandlerInterop>();

		public static IServiceCollection AddBlazorWindows(this IServiceCollection serviceCollection, Action<BlazorWindowOptions> options)
			=> serviceCollection.AddScoped<IWindowManagement, WindowManagementImpl>()
				.AddScoped<WindowHandlerInterop>()
				.Configure(options);

	}
}
