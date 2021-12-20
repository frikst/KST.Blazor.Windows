using System;
using KST.Blazor.Windows.Abstractions;
using KST.Blazor.Windows.Internal;
using KST.Blazor.Windows.Internal.Interop;
using Microsoft.Extensions.DependencyInjection;

namespace KST.Blazor.Windows
{
	/// <summary>
	/// Service collection extensions for registering KST.Blazor.Windows library
	/// </summary>
	public static class BlazorWindowsServiceExtensions
	{
		/// <summary>
		/// Register KST.Blazor.Windows library
		/// </summary>
		/// <param name="serviceCollection">The <see cref="IServiceCollection" /> to add services to.</param>
		/// <returns>A reference to this instance after the operation has completed.</returns>
		public static IServiceCollection AddBlazorWindows(this IServiceCollection serviceCollection)
			=> serviceCollection.AddScoped<IWindowManagement, WindowManagementImpl>()
				.AddScoped<WindowHandlerInterop>();

		/// <summary>
		/// Register KST.Blazor.Windows library
		/// </summary>
		/// <param name="serviceCollection">The <see cref="IServiceCollection" /> to add services to.</param>
		/// <param name="configure">The library configuration</param>
		/// <returns>A reference to this instance after the operation has completed.</returns>
		public static IServiceCollection AddBlazorWindows(this IServiceCollection serviceCollection, Action<BlazorWindowOptions> configure)
			=> serviceCollection.AddScoped<IWindowManagement, WindowManagementImpl>()
				.AddScoped<WindowHandlerInterop>()
				.Configure(configure);

	}
}
