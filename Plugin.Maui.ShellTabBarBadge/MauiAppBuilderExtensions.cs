using Microsoft.Maui.Hosting;
using Microsoft.Maui.Controls;

namespace Plugin.Maui.ShellTabBarBadge;

/// <summary>
/// Provides extension methods for configuring TabBarBadge in a .NET MAUI app.
/// </summary>
public static class MauiAppBuilderExtensions
{
    /// <summary>
    /// Enables TabBarBadge support in the current MAUI application.
    /// </summary>
    /// <param name="builder">
    /// The <see cref="MauiAppBuilder"/> instance used to configure the app.
    /// </param>
    /// <param name="configure">
    /// An optional delegate to configure default <see cref="BadgeOptions"/> 
    /// (e.g., default colors, alignment, or style).  
    /// Example:
    /// <code>
    /// builder.UseTabBarBadge(options =>
    /// {
    ///     options.Color = Colors.Purple;
    ///     options.HorizontalAlignment = HorizontalAlignment.Left;
    /// });
    /// </code>
    /// </param>
    /// <returns>
    /// The same <see cref="MauiAppBuilder"/> instance, for chaining further configuration.
    /// </returns>
    public static MauiAppBuilder UseTabBarBadge(
        this MauiAppBuilder builder,
        Action<BadgeOptions>? configure = null)
    {
        var options = new BadgeOptions();
        configure?.Invoke(options);
        TabBarBadge.Configure(options);

#if IOS || MACCATALYST
            builder.ConfigureMauiHandlers(handlers =>
            {
                // Register our custom renderer for Shell
                handlers.AddHandler<Shell, TabBarBadgeRenderer>();
            });
#endif

        return builder;
    }
}