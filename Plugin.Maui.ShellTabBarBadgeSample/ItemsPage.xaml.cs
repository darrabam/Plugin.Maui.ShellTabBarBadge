using Plugin.Maui.ShellTabBarBadge;

namespace Plugin.Maui.ShellTabBarBadgeSample;

public partial class ItemsPage : ContentPage
{
    Color textColor = Colors.Green;
    string text = "🍕";
    int fontSize = 11;
    public ItemsPage()
	{
		InitializeComponent();
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        TabBarBadge.Set(2, style: BadgeStyle.Hidden);
    }

    private void EmojiSelected(object sender, EventArgs e)
    {
        var btn = sender as Button;
        if (btn == null) return;
        this.text = btn.Text;

        TabBarBadge.Set(2, text: this.text, textColor: this.textColor, color: Colors.Transparent, fontSize: this.fontSize);
    }

    private void OnFontColorClicked(object sender, EventArgs e)
    {
        var btn = sender as Button;
        if (btn == null) return;
            this.textColor = btn.BackgroundColor;

        TabBarBadge.Set(2, text: this.text, textColor: this.textColor , color: Colors.Transparent, fontSize: this.fontSize);
    }

    private void FontSizeChanged(object sender, ValueChangedEventArgs e)
    {
        this.fontSize = (int)e.NewValue;
        TabBarBadge.Set(2, text: this.text, textColor: this.textColor, color: Colors.Transparent, fontSize: this.fontSize);
    }
}