using Maui.Interface;
using Maui.ViewModel.Tarefa;

namespace Maui.View.Tarefa;

public partial class ExibirTarefas : ContentPage
{
    private readonly ExibirTarefasViewModel exibirTarefasViewModel;
    public ExibirTarefas(ITarefaService _iTarefaService)
	{
		InitializeComponent();

        exibirTarefasViewModel = new ExibirTarefasViewModel(_iTarefaService);
        BindingContext = exibirTarefasViewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await exibirTarefasViewModel.InitializeAsync();
    }
}