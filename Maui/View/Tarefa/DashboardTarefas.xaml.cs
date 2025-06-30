using Maui.Interface;
using Maui.ViewModel.Tarefa;

namespace Maui.View.Tarefa;

public partial class DashboardTarefas : ContentPage
{
    private readonly DashboardTarefasViewModel dashboardTarefasViewModel;

    public DashboardTarefas(ITarefaService _iTarefaService)
    {
        InitializeComponent();

        dashboardTarefasViewModel = new DashboardTarefasViewModel(_iTarefaService); 
        BindingContext = dashboardTarefasViewModel; 
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await dashboardTarefasViewModel.InitializeAsync(); 
    }
}