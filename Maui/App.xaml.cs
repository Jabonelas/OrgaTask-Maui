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
            return new Window(new LoginPage())
            {
                Width = 400,
                Height = 600,
                Title = "OrgaTask"
            };

        }
    }
}