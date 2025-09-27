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

            Badge.Set(0, "New", color: Colors.Purple);
            Badge.Set(1, "9");
            Badge.Set(2, "🍕", textColor: Colors.Green, color: Colors.Transparent);
            Badge.Set(3, style: BadgeStyle.Dot, color: Colors.Blue);
        }
    }
}
