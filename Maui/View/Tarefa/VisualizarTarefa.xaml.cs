using Maui.Interface;
using Maui.ViewModel.Tarefa;

namespace Maui.View.Tarefa;

public partial class VisualizarTarefa : ContentPage
{
	public VisualizarTarefa(ITarefaService _iTarefaService)
	{
		InitializeComponent();

        BindingContext = new VisualizarTarefaViewModel(_iTarefaService);

    }
}