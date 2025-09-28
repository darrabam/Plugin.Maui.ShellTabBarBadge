using Plugin.Maui.ShellTabBarBadge;

namespace Plugin.Maui.ShellTabBarBadgeSample;

public partial class HomePage : ContentPage
{
	public HomePage()
	{
		InitializeComponent();
	}
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        TabBarBadge.Set(0, "New", textColor: Colors.Purple, color: Colors.Transparent);
    }

    private void OnResetBadgesClicked(object sender, EventArgs e)
    {
        TabBarBadge.Set(0, "New", color: Colors.Purple);
        TabBarBadge.Set(1, "9");
        TabBarBadge.Set(2, "🍕", textColor: Colors.Green, color: Colors.Transparent);
        TabBarBadge.Set(3, style: BadgeStyle.Dot, color: Colors.Blue);
    }
}