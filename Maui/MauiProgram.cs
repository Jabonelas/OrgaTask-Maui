using CommunityToolkit.Maui;
using Maui.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;

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
                }).UseMauiCommunityToolkit();

            builder.ConfigureLifecycleEvents(events => {
#if ANDROID
                events.AddAndroid(android => {
                    android.OnCreate((activity, bundle) => {
                        // Cor da StatusBar (Azul claro no exemplo)
                        activity.Window?.SetStatusBarColor(Android.Graphics.Color.Rgb(0, 157, 224));

                        // Cor dos ícones (Branco ou Preto)
                        activity.Window?.SetNavigationBarColor(Android.Graphics.Color.Rgb(0, 157, 224));
                    });
                });
#endif
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