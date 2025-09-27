using Microsoft.Maui.Hosting;
using Microsoft.Maui.Controls;

namespace Plugin.Maui.ShellTabBarBadge;

public static class MauiAppBuilderExtensions
{
    public static MauiAppBuilder UseTabBarBadge(
        this MauiAppBuilder builder,
        Action<BadgeOptions>? configure = null)
    {
        var options = new BadgeOptions();
        configure?.Invoke(options);
        Badge.Configure(options);

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
