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
        Badge.Set(0, "New", textColor: Colors.Purple, color: Colors.Transparent);
    }

    private void OnResetBadgesClicked(object sender, EventArgs e)
    {
        Badge.Set(0, "New", color: Colors.Purple);
        Badge.Set(1, "9");
        Badge.Set(2, "🍕", textColor: Colors.Green, color: Colors.Transparent);
        Badge.Set(3, style: BadgeStyle.Dot, color: Colors.Blue);
    }
}