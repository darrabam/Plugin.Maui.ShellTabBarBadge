using Microsoft.Extensions.Logging;
using Plugin.Maui.ShellTabBarBadge;

namespace Plugin.Maui.ShellTabBarBadgeSample
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseTabBarBadge(options =>
                {
                    // Customize default badge options here
                    options.Color = Colors.Red;
                    options.TextColor = Colors.White;
                    // options.AnchorX = 0;
                    // options.AnchorY = 0;
                })
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            
            return builder.Build();
        }
    }
}
