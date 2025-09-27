using Plugin.Maui.ShellTabBarBadge;

namespace Plugin.Maui.ShellTabBarBadgeSample;

public partial class EmailsPage : ContentPage
{
	public EmailsPage()
	{
		InitializeComponent();
	}

    private void OnIncrementBadgeClicked(object sender, EventArgs e)
    {
        App.badgeCounter++;
        Badge.Set(1, App.badgeCounter.ToString(), textColor: Colors.White, color: Colors.Red);
    }

    private void OnDecrementBadgeClicked(object sender, EventArgs e)
    {
        App.badgeCounter--;
        Badge.Set(1, App.badgeCounter.ToString(), textColor: Colors.White, color: Colors.Red);
    }
}