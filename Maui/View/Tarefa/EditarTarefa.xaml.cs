using Maui.Interface;
using Maui.ViewModel.Tarefa;

namespace Maui.View.Tarefa;

public partial class EditarTarefa : ContentPage
{
    public EditarTarefa(ITarefaService _iTarefaService, INotificacaoService _iNotificacaoService)
    {
        InitializeComponent();

        BindingContext = new EditarTarefaViewModel(_iTarefaService, _iNotificacaoService);
    }
}