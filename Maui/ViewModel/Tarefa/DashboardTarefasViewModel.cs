using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Maui.DTOs.Tarefa;
using Maui.Interface;
using System.Collections.ObjectModel;

namespace Maui.ViewModel.Tarefa
{
    public partial class DashboardTarefasViewModel : ObservableObject
    {
        private readonly ITarefaService iTarefaService;

        [ObservableProperty]
        private decimal porcentagemConcluida;

        [ObservableProperty]
        private string frasePorcentagemConcluida;

        [ObservableProperty]
        private string qtdTarefasPrioritarias = "0 item";

        [ObservableProperty]
        private Color corStatus;

        [ObservableProperty]
        private TarefaQtdStatusDTO tarefaQtdStatusDTO;

        [ObservableProperty]
        private bool listaCarregada;

        [ObservableProperty]
        private bool recarregarPage;

        [ObservableProperty]
        private ObservableCollection<TarefaPrioridadeAlta> listaTarefaPrioridadeAlta;

        public DashboardTarefasViewModel(ITarefaService _iTarefaService)
        {
            iTarefaService = _iTarefaService;

            ListaTarefaPrioridadeAlta = new ObservableCollection<TarefaPrioridadeAlta>();

            TarefaQtdStatusDTO = new TarefaQtdStatusDTO();
        }

        public async Task InitializeAsync()
        {
            await CarregarDadosAsync();
        }

        [RelayCommand]
        private async Task CarregarDadosAsync()
        {
            await PreencherPrioridadeEProgressoTotalAsync();

            await PreencherTarefasPrioritariasAsync();
        }

        private async Task PreencherPrioridadeEProgressoTotalAsync()
        {
            try
            {
                (bool Sucesso, string ErrorMessagem, TarefaQtdStatusDTO TarefaQtdStatusDTO) = await iTarefaService.BuscarQtdStatusTarefaAsync();

                if (Sucesso)
                {
                    this.TarefaQtdStatusDTO = TarefaQtdStatusDTO;

                    PorcentagemConcluida = TarefaQtdStatusDTO.PorcentagemConcluidas / 100;

                    FrasePorcentagemConcluida = $"Você completou {TarefaQtdStatusDTO.PorcentagemConcluidas}% das tarefas cadastradas";
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Atenção!", $"{ErrorMessagem}", "OK");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao exibir dados do dashboard: {ex.Message}");

                await Application.Current.MainPage.DisplayAlert("Atenção!",
                       "Ocorreu um erro interno. Nossa equipe já foi notificada.", "OK");
            }
        }

        private async Task PreencherTarefasPrioritariasAsync()
        {
            try
            {
                ListaCarregada = false;

                (bool Sucesso, string ErrorMessagem, List<TarefaPrioridadeAltaDTO> ListaTarefaPrioridadeAltaDTO) =
                    await iTarefaService.BuscarTarefasPrioridadeAltaAsync();

                if (Sucesso && ListaTarefaPrioridadeAltaDTO != null)
                {
                    ListaTarefaPrioridadeAlta.Clear();

                    SetandoQtdTarefasPrioritarias(ListaTarefaPrioridadeAltaDTO.Count);

                    foreach (var tarefa in ListaTarefaPrioridadeAltaDTO)
                    {
                        ListaTarefaPrioridadeAlta.Add(CriarItemTarefa(tarefa));
                    }
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Atenção!", $"{ErrorMessagem}", "OK");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao exibir dados do dashboard: {ex.Message}");

                await Application.Current.MainPage.DisplayAlert("Atenção!",
                      "Ocorreu um erro interno. Nossa equipe já foi notificada.", "OK");
            }
            finally
            {
                ListaCarregada = true;
            }
        }

        private TarefaPrioridadeAlta CriarItemTarefa(TarefaPrioridadeAltaDTO tarefa)
        {
            return new TarefaPrioridadeAlta
            {
                Id = tarefa.Id,
                Titulo = tarefa.Titulo,
                Data = tarefa.Data,
                Prazo = tarefa.Prazo,
                DescricaoPrazo = ObterDescricaoPrazo(tarefa.Prazo),
                CorStatus = SetarCorStatus(tarefa.Status),
                Status = tarefa.Status
            };
        }

        private string ObterDescricaoPrazo(int _prazo)
        {
            return _prazo < 0
                ? $"Prazo: {Math.Abs(_prazo)} dia(s) em atraso."
                : $"Prazo:{_prazo} dia(s) restante(s).";
        }

        private void SetandoQtdTarefasPrioritarias(int _qtdTarefasPrioritarias)
        {
            QtdTarefasPrioritarias = _qtdTarefasPrioritarias == 1 ? "1 item" : $"{_qtdTarefasPrioritarias} Itens";
        }
        private Color SetarCorStatus(string _status)
        {
            switch (_status)
            {
                case "Concluído":
                    return Color.FromArgb("#61C377");

                case "Em Progresso":
                    return Color.FromArgb("#48A6D4");

                case "Pendente":
                    return Color.FromArgb("#A0A0A1");
            }

            return Colors.Gray;
        }

        public class TarefaPrioridadeAlta
        {
            public int Id { get; set; }
            public string Titulo { get; set; }
            public string Data { get; set; }
            public int Prazo { get; set; }
            public string DescricaoPrazo { get; set; }
            public Color CorStatus { get; set; }
            public string Status { get; set; }

        }


        [RelayCommand]
        private async Task RecarregarDashboard()
        {
            try
            {
                RecarregarPage = true;

                await CarregarDadosAsync();
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
        private async Task TarefaSelecionada(int id)
        {
            await Shell.Current.GoToAsync($"///VisualizarTarefa?idTarefa={id}&origin=DashboardTarefas");
        }
    }
}
