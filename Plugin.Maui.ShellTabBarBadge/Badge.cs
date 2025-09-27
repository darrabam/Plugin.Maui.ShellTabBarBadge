namespace Plugin.Maui.ShellTabBarBadge;

public static partial class Badge
{
    static BadgeOptions _options = new();

    public static void Configure(BadgeOptions options)
        => _options = options;

    public static void Set(
        int tabIndex,
        string? text = null,
        Color? textColor = null,
        Color? color = null,
        int? anchorX = null,
        int? anchorY = null,
        BadgeStyle? style = null,
        HorizontalAlignment? horizontal = null,
        VerticalAlignment? vertical = null,
        double? fontSize = null)
    {
        if (style == BadgeStyle.Hidden)
        {
            HideImpl(tabIndex);
            return;
        }

        var finalStyle = style ?? _options.Style;
        bool isDot = finalStyle == BadgeStyle.Dot;

        ShowImpl(
            tabIndex,
            isDot,
            isDot ? null : text,
            isDot ? Colors.Transparent : (textColor ?? _options.TextColor),
            color ?? _options.Color,
            anchorX ?? _options.AnchorX,
            anchorY ?? _options.AnchorY,
            horizontal ?? _options.HorizontalAlignment,
            vertical ?? _options.VerticalAlignment,
            fontSize ?? _options.FontSize);
    }

    // Update partial signature
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
        double fontSize);

    static partial void HideImpl(int tabIndex);
}
