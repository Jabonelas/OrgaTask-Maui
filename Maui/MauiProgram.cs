using Maui.Interface;
using Maui.Service;
using Maui.View;
using Microsoft.Extensions.Logging;

namespace Maui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("SEGOEUIB.ttf", "SegoeUIBold");
                    fonts.AddFont("SEGOEUI.ttf", "SegoeUI");
                });


            // Registre os serviços
            builder.Services.AddSingleton<IUsuarioService, UsuarioService>();
            builder.Services.AddSingleton<ITarefaService, TarefaService>();

            // Registre ViewModels e Views
            builder.Services.AddTransient<LoginPage>();
            //builder.Services.AddTransient<MainViewModel>();


#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}