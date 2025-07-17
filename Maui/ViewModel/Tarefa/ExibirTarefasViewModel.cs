using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Maui.DTOs.Tarefa;
using Maui.Interface;
using System.Collections.ObjectModel;

namespace Maui.ViewModel.Tarefa
{
    [QueryProperty(nameof(Status), "status")]
    public partial class ExibirTarefasViewModel : ObservableObject
    {
        private readonly ITarefaService iTarefaService;

        [ObservableProperty]
        private int currentPage = 1;

        [ObservableProperty]
        private decimal porcentagemConcluida;

        [ObservableProperty]
        private string frasePorcentagemConcluida;

        [ObservableProperty]
        private string qtdTarefasPrioritarias = "0 item";

        [ObservableProperty]
        private string status;

        [ObservableProperty]
        private string titulo;

        [ObservableProperty]
        private TarefaQtdStatusDTO tarefaQtdStatusDTO;

        [ObservableProperty]
        private bool listaCarregada;

        [ObservableProperty]
        private bool tarefaCadastradas;

        [ObservableProperty]
        private bool recarregarPage;

        [ObservableProperty]
        private ObservableCollection<Tarefas> listaTarefas;

        [ObservableProperty]
        private bool isLoadingMore;

        [ObservableProperty]
        private bool hasMoreItems = true;

        [ObservableProperty]
        private bool hasNoMoreItems;

        public ExibirTarefasViewModel(ITarefaService _iTarefaService)
        {
            iTarefaService = _iTarefaService;
            ListaTarefas = new ObservableCollection<Tarefas>();
            TarefaQtdStatusDTO = new TarefaQtdStatusDTO();
            HasMoreItems = true; // Explicit initialization
        }

         partial void OnStatusChanged(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                status = value;
                _ = CarregarDadosAsync();
            }
        }

         partial void OnHasMoreItemsChanged(bool value)
        {
            HasNoMoreItems = !value;
        }

        public async Task InitializeAsync()
        {
            if (string.IsNullOrEmpty(Status))
            {
                await CarregarDadosAsync();
            }
        }

        [RelayCommand]
        private async Task CarregarDadosAsync()
        {
            SetandoTitulo(Status);
            await PreencherTarefasAsync();
        }

        [RelayCommand]
        private async Task RecarregarPagina()
        {
            try
            {
                RecarregarPage = true;
                await PreencherTarefasAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao exibir dados do dashboard: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Erro", "Falha ao recarregar Dashboard.", "OK");
            }
            finally
            {
                RecarregarPage = false;
            }
        }

        [RelayCommand]
        private async Task NovaTarefa()
        {
            await Shell.Current.GoToAsync($"///CadastrarTarefa");
        }

        private async Task PreencherTarefasAsync(bool loadMore = false)
        {
            Console.WriteLine("Iniciando PreencherTarefasAsync");

            try
            {
                if (!loadMore)
                {
                    ListaCarregada = false;
                    CurrentPage = 1;
                    HasMoreItems = true;
                    ListaTarefas.Clear();
                }
                else
                {
                    if (IsLoadingMore || !HasMoreItems)
                    {
                        return;
                    }
                    IsLoadingMore = true;
                }

                int pageSize = 12;

                (bool Sucesso, string ErrorMessagem, List<TarefaConsultaDTO> ListaTarefaConsultaDTO, int totalCount) =
                    await iTarefaService.ObterTarefasPaginadasAsync(CurrentPage, pageSize, Status);

                Console.WriteLine("Iniciando PreencherTarefasAsync2");

                if (Sucesso && !ListaTarefaConsultaDTO?.Any() == true)
                {
                    TarefaCadastradas = true;
                }
                else
                {
                    TarefaCadastradas = false;
                }

                if (Sucesso && ListaTarefaConsultaDTO?.Any() == true)
                {
                    var newItems = ListaTarefaConsultaDTO.Select(CriarItemTarefa).ToList();
                    foreach (var item in newItems)
                    {
                        ListaTarefas.Add(item);
                    }

                    HasMoreItems = (CurrentPage * pageSize) < totalCount;
                    CurrentPage++;
                }
                else if (!ListaTarefaConsultaDTO?.Any() == true && loadMore)
                {
                    HasMoreItems = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao exibir lista de tarefas: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Atenção!",
                    "Ocorreu um erro interno. Nossa equipe já foi notificada.", "OK");
            }
            finally
            {
                ListaCarregada = true;
                IsLoadingMore = false;
            }
        }

        private void SetandoTitulo(string? _status)
        {
            switch (_status)
            {
                case "Concluído":
                    Titulo = "Todas as Minhas Tarefas Concluídas";
                    return;

                case "Em_Progresso":
                    Titulo = "Todas as Minhas Tarefas Em Progresso";
                    return;

                case "Pendente":
                    Titulo = "Todas as Minhas Tarefas Pendentes";
                    return;
            }

            Titulo = "Todas as Minhas Tarefas";
        }

        [RelayCommand]
        private async Task LoadMoreItems()
        {
            await PreencherTarefasAsync(true);
        }

        private Tarefas CriarItemTarefa(TarefaConsultaDTO tarefa)
        {
            return new Tarefas
            {
                Id = tarefa.Id,
                Titulo = tarefa.Titulo,
                Data = tarefa.DataCriacao,
                Prazo = tarefa.Prazo,
                DescricaoPrazo = ObterDescricaoPrazo(tarefa.Prazo),
                Descricao = tarefa.Descricao,
                Prioridade = $" Prioridade: {tarefa.Prioridade}",
                CorPrioridade = SetarCorPrioridade(tarefa.Prioridade),
                IconePrioridade = SetarIconePrioridade(tarefa.Prioridade),
                CorStatus = SetarCorStatus(tarefa.Status),
                IconeStatus = SetarIconeStatus(tarefa.Status),
                Status = $" Status: {tarefa.Status}",
                CorFundo = SetarCorFundo(tarefa.Status)
            };
        }

        private string ObterDescricaoPrazo(int _prazo)
        {
            return _prazo < 0
                ? $"Prazo: {Math.Abs(_prazo)} dia(s) em atraso."
                : $"Prazo: {_prazo} dia(s) restante(s).";
        }

        private Color SetarCorPrioridade(string _prioridade)
        {
            switch (_prioridade)
            {
                case "Baixa":
                    return Color.FromArgb("#43A047");

                case "Média":
                    return Color.FromArgb("#FB8C00");

                case "Alta":
                    return Color.FromArgb("#E53935");
            }

            return Colors.Gray;
        }

        private Color SetarCorStatus(string _status)
        {
            switch (_status)
            {
                case "Concluído":
                    return Color.FromArgb("#66BB6A");

                case "Em Progresso":
                    return Color.FromArgb("#FBC02D");

                case "Pendente":
                    return Color.FromArgb("#D32F2F");
            }

            return Colors.Gray;
        }

        private ImageSource SetarIconePrioridade(string _prioridade)
        {
            switch (_prioridade)
            {
                case "Baixa":
                    return ImageSource.FromFile("prioridade_baixa.png");

                case "Média":
                    return ImageSource.FromFile("prioridade_media.png");

                case "Alta":
                    return ImageSource.FromFile("prioridade_alta.png");
            }

            return null;
        }

        private ImageSource SetarIconeStatus(string _status)
        {
            switch (_status)
            {
                case "Concluído":
                    return ImageSource.FromFile("status_concluido.png");

                case "Em Progresso":
                    return ImageSource.FromFile("status_em_progresso.png");

                case "Pendente":
                    return ImageSource.FromFile("status_pendente.png");
            }

            return null;
        }

        private Color SetarCorFundo(string _status)
        {
            switch (_status)
            {
                case "Concluído":
                    return Color.FromArgb("#f4fbf6");

                case "Em Progresso":
                    return Color.FromArgb("#fff8f3");

                case "Pendente":
                    return Color.FromArgb("#fdf5f6");
            }

            return Colors.Gray;
        }

        public class Tarefas
        {
            public int Id { get; set; }
            public string Titulo { get; set; }
            public string Descricao { get; set; }
            public string Data { get; set; }
            public int Prazo { get; set; }
            public string DescricaoPrazo { get; set; }
            public string Prioridade { get; set; }
            public ImageSource IconePrioridade { get; set; }
            public Color CorPrioridade { get; set; }
            public string Status { get; set; }
            public ImageSource IconeStatus { get; set; }
            public Color CorStatus { get; set; }
            public Color CorFundo { get; set; }
        }

        [RelayCommand]
        private async Task Recarregar()
        {
            try
            {
                RecarregarPage = true;
                await CarregarDadosAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao exibir lista de tarefas: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Erro", "Falha ao recarregar Dashboard.", "OK");
            }
            finally
            {
                RecarregarPage = false;
            }
        }

        [RelayCommand]
        private async Task Visualizar(int id)
        {
            string exibirTarefa = Uri.EscapeDataString($"ExibirTarefas?status={Status}");
            await Shell.Current.GoToAsync($"///VisualizarTarefa?idTarefa={id}&origin={exibirTarefa}");
        }

        [RelayCommand]
        private async Task EditarTarefa(int id)
        {
            string editarTarefa = Uri.EscapeDataString($"ExibirTarefas?status={Status}");
            await Shell.Current.GoToAsync($"///EditarTarefa?idTarefa={id}&origin={editarTarefa}");
        }

        [RelayCommand]
        private async Task ExcluirTarefa(int id)
        {
            var resposta = await Application.Current.MainPage.DisplayAlert("Atenção!",
                "Tem certeza que deseja excluir esta tarefa?",
                "Sim", "Não");

            if (resposta)
            {
                await iTarefaService.DeletarTarefaAsync(id);
                await RecarregarPagina();
            }
        }
    }
}