namespace Plugin.Maui.ShellTabBarBadge;

public static partial class TabBarBadge
{
    static BadgeOptions _options = new();

    /// <summary>
    /// Applies global default options for all badges in the application.
    /// </summary>
    /// <param name="options">
    /// A <see cref="BadgeOptions"/> instance specifying default style, colors,
    /// alignment, and font size to be used when creating badges.  
    /// These values are applied unless explicitly overridden when calling <see cref="Set"/>.
    /// </param>
    public static void Configure(BadgeOptions options)
        => _options = options;
    /// <summary>
    /// Sets or updates a badge on the specified Shell tab bar item.
    /// </summary>
    /// <param name="tabIndex">
    /// The zero-based index of the tab item where the badge should appear.
    /// </param>
    /// <param name="text">
    /// The text or emoji to display inside the badge.  
    /// Ignored when <paramref name="style"/> is <see cref="BadgeStyle.Dot"/>.
    /// </param>
    /// <param name="textColor">
    /// The foreground color of the badge text. Defaults to <see cref="BadgeOptions.TextColor"/>.
    /// </param>
    /// <param name="color">
    /// The badge background color (pill for text/number, fill for dot).  
    /// Defaults to <see cref="BadgeOptions.Color"/>.
    /// </param>
    /// <param name="anchorX">
    /// Optional horizontal offset in device-independent pixels relative to the anchor view (icon or label).  
    /// Positive values move the badge to the right; negative values move it left.
    /// </param>
    /// <param name="anchorY">
    /// Optional vertical offset in device-independent pixels relative to the anchor view.  
    /// Positive values move the badge down; negative values move it up.
    /// </param>
    /// <param name="style">
    /// The badge style to use: <see cref="BadgeStyle.Text"/> (default),  
    /// <see cref="BadgeStyle.Dot"/> (tiny circle), or <see cref="BadgeStyle.Hidden"/>.
    /// </param>
    /// <param name="horizontal">
    /// Horizontal alignment of the badge relative to the anchor:  
    /// <see cref="HorizontalAlignment.Left"/>, <see cref="HorizontalAlignment.Center"/>, or <see cref="HorizontalAlignment.Right"/>.
    /// Defaults to <see cref="BadgeOptions.HorizontalAlignment"/>.
    /// </param>
    /// <param name="vertical">
    /// Vertical alignment of the badge relative to the anchor:  
    /// <see cref="VerticalAlignment.Top"/>, <see cref="VerticalAlignment.Center"/>, or <see cref="VerticalAlignment.Bottom"/>.
    /// Defaults to <see cref="BadgeOptions.VerticalAlignment"/>.
    /// </param>
    /// <param name="fontSize">
    /// Font size in device-independent units (points) for text badges. Ignored for dot style.  
    /// Defaults to <see cref="BadgeOptions.FontSize"/>.
    /// </param>
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
