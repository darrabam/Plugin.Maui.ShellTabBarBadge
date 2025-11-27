using Plugin.Maui.ShellTabBarBadge;

namespace Plugin.Maui.ShellTabBarBadgeSample;

public partial class ProfilePage : ContentPage
{
    int badgeAnchorX = 0;
    int badgeAnchorY = 0;
    HorizontalAlignment horizontalAlignment = HorizontalAlignment.Right;
    VerticalAlignment verticalAlignment = VerticalAlignment.Top;

	public ProfilePage()
	{
		InitializeComponent();
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();
    }

    private void TopLeftClicked(object sender, EventArgs e)
    {
        this.horizontalAlignment = HorizontalAlignment.Left;
        this.verticalAlignment = VerticalAlignment.Top;
        UpdateBadge();
    }

    private void TopCenterClicked(object sender, EventArgs e)
    {
        this.horizontalAlignment = HorizontalAlignment.Center;
        this.verticalAlignment = VerticalAlignment.Top;
        UpdateBadge();
    }

    private void TopRightClicked(object sender, EventArgs e)
    {
        this.horizontalAlignment = HorizontalAlignment.Right;
        this.verticalAlignment = VerticalAlignment.Top;
        UpdateBadge();
    }

    private void CenterLeftClicked(object sender, EventArgs e)
    {
        this.horizontalAlignment = HorizontalAlignment.Left;
        this.verticalAlignment = VerticalAlignment.Center;
        UpdateBadge();
    }

    private void CenterClicked(object sender, EventArgs e)
    {
        this.horizontalAlignment = HorizontalAlignment.Center;
        this.verticalAlignment = VerticalAlignment.Center;
        UpdateBadge();
    }

    private void CenterRightClicked(object sender, EventArgs e)
    {
        this.horizontalAlignment = HorizontalAlignment.Right;
        this.verticalAlignment = VerticalAlignment.Center;
        UpdateBadge();
    }

    private void BottomLeftClicked(object sender, EventArgs e)
    {
        this.horizontalAlignment = HorizontalAlignment.Left;
        this.verticalAlignment = VerticalAlignment.Bottom;
        UpdateBadge();
    }

    private void BottomCenterClicked(object sender, EventArgs e)
    {
        this.horizontalAlignment = HorizontalAlignment.Center;
        this.verticalAlignment = VerticalAlignment.Bottom;
        UpdateBadge();
    }

    private void BottomRightClicked(object sender, EventArgs e)
    {
        this.horizontalAlignment = HorizontalAlignment.Right;
        this.verticalAlignment = VerticalAlignment.Bottom;
        UpdateBadge();
    }

    private void DecrementAnchorYClicked(object sender, EventArgs e)
    {
        this.badgeAnchorY--;
        UpdateBadge();
    }
    private void UpdateBadge()
    {
        TabBarBadge.Set(3, style: BadgeStyle.Dot, color: Colors.Blue, horizontal: this.horizontalAlignment, vertical: this.verticalAlignment, anchorX: this.badgeAnchorX, anchorY: this.badgeAnchorY);
    }

    private void DecrementAnchorXClicked(object sender, EventArgs e)
    {
        this.badgeAnchorX--;
        UpdateBadge();
    }

    private void IncrementAnchorXClicked(object sender, EventArgs e)
    {
        this.badgeAnchorX++;
        UpdateBadge();
    }

    private void IncrementAnchorYClicked(object sender, EventArgs e)
    {
        this.badgeAnchorY++;
        UpdateBadge();
    }

    private void RestAnchorClicked(object sender, EventArgs e)
    {
        this.badgeAnchorX = 0;
        this.badgeAnchorY = 0;
        UpdateBadge();
    }
}