using Maui.Interface;
using Maui.ViewModel.Tarefa;

namespace Maui.View.Tarefa;

public partial class CadastrarTarefa : ContentPage
{
    public CadastrarTarefa(ITarefaService _iTarefaService, INotificacaoService _iNotificacaoService)
    {
        InitializeComponent();

        BindingContext = new CadastrarTarefaViewModel(_iTarefaService, _iNotificacaoService);
    }


}