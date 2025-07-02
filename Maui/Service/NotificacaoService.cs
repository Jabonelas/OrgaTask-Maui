using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Maui.Interface;

namespace Maui.Service
{
    public class NotificacaoService : INotificacaoService
    {


        public async Task MostrarNotificacaoAsync(string message)
        {
            await MostrarNotificacaoAsync(message, ToastDuration.Short, 14);
        }

        public async Task MostrarNotificacaoAsync(string message, ToastDuration duration, double fontSize)
        {
            try
            {
                if (MainThread.IsMainThread)
                {
                    await Toast.Make(message, duration, fontSize).Show();
                }
                else
                {
                    await MainThread.InvokeOnMainThreadAsync(async () =>
                    {
                        await Toast.Make(message, duration, fontSize).Show();
                    });
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Erro ao exibir notificação: {ex.Message}");


            }
        }

    }
}
