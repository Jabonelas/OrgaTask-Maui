namespace Maui
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Cria o AppShell como container de navegação
            MainPage = new AppShell();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var window = base.CreateWindow(activationState);

            window.Title = "OrgaTask";
            window.Width = 400;
            window.Height = 700;

            // Aguarda o Shell estar pronto antes de navegar
            Device.BeginInvokeOnMainThread(async () =>
            {
                await Task.Delay(100); // Pequeno delay para garantir inicialização

                await Shell.Current.GoToAsync("//Login");
            });

            return window;
        }

        //protected override Window CreateWindow(IActivationState? activationState)
        //{
        //    //var loginPage = MauiProgram.Services.GetRequiredService<LoginPage>();
        //    //return new Window(new NavigationPage( loginPage));

        //    var loginPage = MauiProgram.Services.GetRequiredService<LoginPage>();
        //    var navPage = new NavigationPage(loginPage);

        //    var window = new Window(navPage)
        //    {
        //        Width = 400,
        //        Height = 700,
        //        Title = "OrgaTask"
        //    };

        //    return window;

        //}
    }
}