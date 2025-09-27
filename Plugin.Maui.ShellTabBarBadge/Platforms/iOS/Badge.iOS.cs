using Microsoft.Maui.Graphics;
using Microsoft.Maui.Platform;
using UIKit;

namespace Plugin.Maui.ShellTabBarBadge;

public static partial class Badge
{
    static partial void ShowImpl(
        int tabIndex,
        bool isDot,
        string? text,
        Color foreground,
        Color background,
        int anchorX,
        int anchorY,
        HorizontalAlignment horizontal,
        VerticalAlignment vertical,
        double fontSize)
    {
        BadgeShellTabBarAppearanceTracker.SetBadge(
            tabIndex,
            isDot,
            text,
            background.ToPlatform(),
            foreground.ToPlatform(),
            anchorX,
            anchorY,
            horizontal,
            vertical,
            fontSize);
    }

    static partial void HideImpl(int tabIndex)
    {
        BadgeShellTabBarAppearanceTracker.SetBadge(
            tabIndex, false, null, null, null, 0, 0,
            HorizontalAlignment.Right, VerticalAlignment.Top, 11);
    }
}
