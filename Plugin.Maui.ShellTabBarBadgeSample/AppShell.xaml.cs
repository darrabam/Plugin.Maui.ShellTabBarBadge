using Plugin.Maui.ShellTabBarBadge;

namespace Plugin.Maui.ShellTabBarBadgeSample
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            this.Loaded += AppShell_Loaded;
        }
        private async void AppShell_Loaded(object sender, EventArgs e)
        {
            // Optional: unsubscribe so it only runs once
            this.Loaded -= AppShell_Loaded;

            // Delay after the Shell has loaded to make sure tabs have been initiated.
            await Task.Delay(500); 

            TabBarBadge.Set(0, "New", color: Colors.Purple);
            TabBarBadge.Set(1, "9");
            TabBarBadge.Set(2, "🍕", textColor: Colors.Green, color: Colors.Transparent);
            TabBarBadge.Set(3, style: BadgeStyle.Dot, color: Colors.Blue);
        }
    }
}
