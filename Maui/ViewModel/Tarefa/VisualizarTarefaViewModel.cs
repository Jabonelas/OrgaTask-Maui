using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Maui.DTOs.Tarefa;
using Maui.Interface;
using Maui.Models;

namespace Maui.ViewModel.Tarefa
{
    [QueryProperty(nameof(IdTarefa), "idTarefa")]
    [QueryProperty(nameof(Origin), "origin")]
    public partial class VisualizarTarefaViewModel : ObservableObject
    {
        private readonly ITarefaService iTarefaService;

        [ObservableProperty]
        private TarefaAlterarDTO tarefaAlterarDTO;

        [ObservableProperty]
        private int idTarefa;

        [ObservableProperty]
        private string origin;

        [ObservableProperty]
        private string iconeCabecalho;

        [ObservableProperty]
        private string tituloCabecalho;

        [ObservableProperty]
        private string subtituloCabecalho;

        [ObservableProperty]
        private bool isEnabled;

        [ObservableProperty]
        private bool mostrarErroTitulo;

        [ObservableProperty]
        private bool mostrarErroPrioridade;

        [ObservableProperty]
        private bool mostrarErroPrazo;

        [ObservableProperty]
        private bool mostrarErroDescricao;

        [ObservableProperty]
        private bool mostrarErroStatus;

        [ObservableProperty]
        private bool mostarBtnCancelar;

        [ObservableProperty]
        private bool mostarBtnSalvar;

        [ObservableProperty]
        private List<ItemCabecalhoTarefa.ItensCabecario> listaItensCabecalho = new List<ItemCabecalhoTarefa.ItensCabecario>();

        [ObservableProperty]
        private List<string> listaDePrioridades = new List<string>();

        [ObservableProperty]
        private List<string> listaDeStatus = new List<string>();

        public VisualizarTarefaViewModel(ITarefaService _iTarefaService)
        {
            iTarefaService = _iTarefaService;

            TarefaAlterarDTO = new TarefaAlterarDTO();

            PreencherCabecario();

            PreencherListaPrioridade();

            PreencherListaStatus();
        }

        private partial void OnIdTarefaChanged(int value)
        {
            _ = CarregarTarefaAsync(value);
        }

        private void PreencherCabecario()
        {
            IconeCabecalho = "detalhe_tarefa.png";
            TituloCabecalho = "Detalhes da Tarefa";
            SubtituloCabecalho = "Visualize todos os detalhes desta tarefa.";

            PreencherListaItensCabecario();
        }

        private void PreencherListaItensCabecario()
        {
            ListaItensCabecalho.AddRange(new[]
            {
                new ItemCabecalhoTarefa.ItensCabecario
                {
                    Icone = "aceitar.png",
                    Texto = "Detalhes completos da tarefa."
                },
                new ItemCabecalhoTarefa.ItensCabecario
                {
                    Icone = "aceitar.png",
                    Texto = "Prioridade e status claros."
                },
                new ItemCabecalhoTarefa.ItensCabecario
                {
                    Icone = "aceitar.png",
                    Texto = "Prazo e descrição detalhados."
                }
            });
        }

        private void PreencherListaPrioridade()
        {
            ListaDePrioridades.Add("Alta");
            ListaDePrioridades.Add("Média");
            ListaDePrioridades.Add("Baixa");
        }

        private void PreencherListaStatus()
        {
            ListaDeStatus.Add("Pendente");
            ListaDeStatus.Add("Em Progresso");
            ListaDeStatus.Add("Concluído");
        }

        private async Task CarregarTarefaAsync(int idTarefa)
        {
            try
            {
                (bool Sucesso, string ErrorMessagem, TarefaAlterarDTO TarefaAlterarDTO) = await iTarefaService.BuscarTarefaAsync(IdTarefa);

                if (Sucesso)
                {
                    this.TarefaAlterarDTO = TarefaAlterarDTO;
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Atenção!", ErrorMessagem, "OK");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao visualizar dados da tarefa: {ex.Message}");

                await Application.Current.MainPage.DisplayAlert("Atenção!",
                    "Ocorreu um erro interno. Nossa equipe já foi notificada.", "OK");
            }
        }

        [RelayCommand]
        private async Task Voltar()
        {
            await Shell.Current.GoToAsync($"//{Origin}");
        }
    }
}