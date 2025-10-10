#if MACCATALYST
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Platform;
using UIKit;

namespace Plugin.Maui.ShellTabBarBadge;

/// <summary>
/// Provides a cross-platform API for showing and managing badges 
/// on .NET MAUI Shell tab bar items.
/// </summary>
public static partial class TabBarBadge
{
    static partial void ShowImpl(
        int tabIndex,
        bool isDot,
        string? text,
        Color textColor,
        Color color,
        int anchorX,
        int anchorY,
        HorizontalAlignment horizontal,
        VerticalAlignment vertical,
        double fontSize)
    {
        // Mac Catalyst uses UIKit, so reuse the iOS badge tracker
        BadgeShellTabBarAppearanceTracker.SetBadge(
            tabIndex,
            isDot,
            text,
            color.ToPlatform(),      // background
            textColor.ToPlatform(),  // foreground
            anchorX,
            anchorY,
            horizontal,
            vertical,
            fontSize);
    }

    static partial void HideImpl(int tabIndex)
    {
        BadgeShellTabBarAppearanceTracker.SetBadge(
            tabIndex,
            false,
            null,
            null,
            null,
            0,
            0,
            HorizontalAlignment.Right,
            VerticalAlignment.Top,
            11);
    }
}
#endif
