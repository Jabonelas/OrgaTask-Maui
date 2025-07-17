using Maui.Interface;
using Maui.ViewModel.Tarefa;

namespace Maui.View.Tarefa;

public partial class ExibirTarefas : ContentPage
{
    private readonly ExibirTarefasViewModel exibirTarefasViewModel;

    private DateTime lastLoadTime = DateTime.MinValue;

    public ExibirTarefas(ITarefaService _iTarefaService)
    {
        try
        {
            InitializeComponent();
            exibirTarefasViewModel = new ExibirTarefasViewModel(_iTarefaService);
            BindingContext = exibirTarefasViewModel;
            MainScrollView.Scrolled += OnScrollViewScrolled;

            // Prevent linker from removing InverseBoolConverter
            //var dummy = new Maui.Helpers.InverseBoolConverter();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error initializing ExibirTarefas: {ex}");
            Application.Current.MainPage.DisplayAlert("Initialization Error", ex.Message, "OK").GetAwaiter().GetResult();
            throw;
        }
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is ExibirTarefasViewModel vm)
        {
            await vm.InitializeAsync();
        }
    }

    private async void OnScrollViewScrolled(object sender, ScrolledEventArgs e)
    {
        if ((DateTime.Now - lastLoadTime).TotalMilliseconds < 1000) return;
        if (exibirTarefasViewModel.IsLoadingMore || !exibirTarefasViewModel.HasMoreItems) return;

        var scrollView = (ScrollView)sender;
        var scrollingSpace = scrollView.ContentSize.Height - scrollView.Height;

        if (scrollingSpace <= 0) return;

        var currentPosition = e.ScrollY;
        var threshold = scrollingSpace * 0.8;

        if (currentPosition >= threshold)
        {
            lastLoadTime = DateTime.Now;
            await exibirTarefasViewModel.LoadMoreItemsCommand.ExecuteAsync(null);
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        MainScrollView.Scrolled -= OnScrollViewScrolled;
        exibirTarefasViewModel.Status = null;
    }
}