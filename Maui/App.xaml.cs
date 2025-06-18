using Maui.View;

namespace Maui
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            //var loginPage = MauiProgram.Services.GetRequiredService<LoginPage>();
            //return new Window(new NavigationPage( loginPage));

            var loginPage = MauiProgram.Services.GetRequiredService<LoginPage>();
            var navPage = new NavigationPage(loginPage);

            var window = new Window(navPage)
            {
                Width = 400,
                Height = 700,
                Title = "OrgaTask"
            };

            return window;

        }
    }
}