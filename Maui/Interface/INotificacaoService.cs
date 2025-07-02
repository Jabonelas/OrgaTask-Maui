using CommunityToolkit.Maui.Core;

namespace Maui.Interface
{
    public interface INotificacaoService
    {
        Task MostrarNotificacaoAsync(string message);
        Task MostrarNotificacaoAsync(string message, ToastDuration duration = ToastDuration.Short, double fontSize = 14);
    }
}
