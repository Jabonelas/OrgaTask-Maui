using Maui.Interface;
using Maui.Service;
using Maui.ViewModel.Tarefa;

namespace Maui.View.Tarefa;

public partial class CadastrarTarefa : ContentPage
{
	public CadastrarTarefa(ITarefaService _iTarefaService)
	{
		InitializeComponent();

        BindingContext = new CadastrarTarefaViewModel(_iTarefaService);
    }
}