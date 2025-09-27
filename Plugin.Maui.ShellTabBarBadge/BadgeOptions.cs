namespace Plugin.Maui.ShellTabBarBadge;

public class BadgeOptions
{
    public BadgeStyle Style { get; set; } = BadgeStyle.Text;
    public Color TextColor { get; set; } = Colors.White;
    public Color Color { get; set; } = Colors.Red;

    public HorizontalAlignment HorizontalAlignment { get; set; } = HorizontalAlignment.Right;
    public VerticalAlignment VerticalAlignment { get; set; } = VerticalAlignment.Top;
    public double FontSize { get; set; } = 11; // default for text badges

    public int AnchorX { get; set; } = 0;
    public int AnchorY { get; set; } = 0;



}
