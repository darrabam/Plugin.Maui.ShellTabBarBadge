using System;

namespace Plugin.Maui.TabBarBadge;

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
        throw new NotImplementedException("TabBarBadge is not implemented on this platform.");
    }

    static partial void HideImpl(int tabIndex)
    {
        throw new NotImplementedException("TabBarBadge is not implemented on this platform.");
    }
}
