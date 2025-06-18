using Maui.Extensions;
using Microsoft.Extensions.Logging;

namespace Maui
{
    public static class MauiProgram
    {
        public static IServiceProvider Services { get; private set; }

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


            builder.Services.AdicionarInjecoesDependencias();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            var app = builder.Build();
            Services = app.Services;
            return app;
        }
    }
}