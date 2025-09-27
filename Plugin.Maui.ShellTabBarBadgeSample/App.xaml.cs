namespace Plugin.Maui.ShellTabBarBadgeSample
{
    public partial class App : Application
    {
        public static int badgeCounter = 9;
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}