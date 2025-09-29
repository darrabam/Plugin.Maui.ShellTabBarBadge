namespace Plugin.Maui.ShellTabBarBadge;

/// <summary>
/// Defines default configuration values for TabBar badges.
/// These values are applied unless overridden when calling <see cref="TabBarBadge.Set"/>.
/// </summary>
public class BadgeOptions
{
    /// <summary>
    /// The default badge style applied when no style is specified in <see cref="TabBarBadge.Set"/>.
    /// Options are <see cref="BadgeStyle.Text"/>, <see cref="BadgeStyle.Dot"/>, or <see cref="BadgeStyle.Hidden"/>.
    /// Default is <see cref="BadgeStyle.Text"/>.
    /// </summary>
    public BadgeStyle Style { get; set; } = BadgeStyle.Text;

    /// <summary>
    /// The default text color for badge content (numbers, text, or symbols).
    /// Ignored for dot style. Default is <see cref="Colors.White"/>.
    /// </summary>
    public Color TextColor { get; set; } = Colors.White;

    /// <summary>
    /// The default background color of the badge.
    /// For text/number badges, this colors the pill background.  
    /// For dot badges, this colors the dot itself.  
    /// Default is <see cref="Colors.Red"/>.
    /// </summary>
    public Color Color { get; set; } = Colors.Red;

    /// <summary>
    /// The default horizontal alignment of the badge relative to its anchor (icon or text).  
    /// Options: <see cref="HorizontalAlignment.Left"/>, <see cref="HorizontalAlignment.Center"/>, <see cref="HorizontalAlignment.Right"/>.  
    /// Default is <see cref="HorizontalAlignment.Right"/>.
    /// </summary>
    public HorizontalAlignment HorizontalAlignment { get; set; } = HorizontalAlignment.Right;

    /// <summary>
    /// The default vertical alignment of the badge relative to its anchor (icon or text).  
    /// Options: <see cref="VerticalAlignment.Top"/>, <see cref="VerticalAlignment.Center"/>, <see cref="VerticalAlignment.Bottom"/>.  
    /// Default is <see cref="VerticalAlignment.Top"/>.
    /// </summary>
    public VerticalAlignment VerticalAlignment { get; set; } = VerticalAlignment.Top;

    /// <summary>
    /// The default font size (in device-independent units) for text badges.  
    /// Ignored for dot style. Default is <c>11</c>.
    /// </summary>
    public double FontSize { get; set; } = 11;

    /// <summary>
    /// The default horizontal offset (in device-independent pixels) applied to badge placement.  
    /// Positive values move the badge to the right, negative values move it to the left.  
    /// Default is <c>0</c>.
    /// </summary>
    public int AnchorX { get; set; } = 0;

    /// <summary>
    /// The default vertical offset (in device-independent pixels) applied to badge placement.  
    /// Positive values move the badge downward, negative values move it upward.  
    /// Default is <c>0</c>.
    /// </summary>
    public int AnchorY { get; set; } = 0;
}