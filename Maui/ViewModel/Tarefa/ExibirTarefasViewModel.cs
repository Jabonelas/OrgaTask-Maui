using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Maui.DTOs.Tarefa;
using Maui.Interface;
using Maui.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maui.ViewModel.Tarefa
{
    public partial class ExibirTarefasViewModel : ObservableObject
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
        private ObservableCollection<Tarefas> listaTarefaPrioridadeAlta;

        public ExibirTarefasViewModel(ITarefaService _iTarefaService)
        {
            iTarefaService = _iTarefaService;

            ListaTarefaPrioridadeAlta = new ObservableCollection<ExibirTarefasViewModel.Tarefas>();

            TarefaQtdStatusDTO = new TarefaQtdStatusDTO();
        }


        public async Task InitializeAsync()
        {
            await CarregarDadosAsync();
        }

        [RelayCommand]
        private async Task CarregarDadosAsync()
        {

            await PreencherTarefasPrioritariasAsync();
        }


        private async Task PreencherTarefasPrioritariasAsync()
        {
            try
            {
                //ListaCarregada = false;

                int pageNumber = 1;
                int pageSize = 12;
                string status = "todas";

                 (bool Sucesso, string ErrorMessagem, List<TarefaConsultaDTO> ListaTarefaConsultaDTO, int totalCount)  = await iTarefaService.ObterTarefasPaginadasAsync( pageNumber, pageSize, status);

                if (Sucesso && ListaTarefaConsultaDTO != null)
                {
                    ListaTarefaPrioridadeAlta.Clear();

                    foreach (var tarefa in ListaTarefaConsultaDTO)
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
                //ListaCarregada = true;
            }
        }

        private Tarefas CriarItemTarefa(TarefaConsultaDTO tarefa)
        {
            return new Tarefas
            {
                Id = tarefa.Id,
                Titulo = tarefa.Titulo,
                Data = tarefa.Data,
                Prazo = tarefa.Prazo,
                DescricaoPrazo = ObterDescricaoPrazo(tarefa.Prazo),
                Descricao = tarefa.Descricao,
                Prioridade = tarefa.Prioridade,
                CorPrioridade = SetarCorStatus(tarefa.Status),
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

        public class Tarefas
        {
            public int Id { get; set; }
            public string Titulo { get; set; }
            public string Descricao { get; set; }
            public string Data { get; set; }
            public int Prazo { get; set; }
            public string DescricaoPrazo { get; set; }
            public string Prioridade { get; set; }
            public Color CorPrioridade { get; set; }
            public Color CorStatus { get; set; }
            public string Status { get; set; }

        }


        [RelayCommand]
        private async Task RecarregarDashboard()
        {
            try
            {
                //RecarregarPage = true;

                await CarregarDadosAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao exibir dados do dashboard: {ex.Message}");

                await Application.Current.MainPage.DisplayAlert("Erro", "Falha ao recarregar Dashboard.", "OK");
            }
            finally
            {
                //RecarregarPage = false;
            }

        }


        [RelayCommand]
        private async Task TarefaSelecionada(int id)
        {
            await Shell.Current.GoToAsync($"///VisualizarTarefa?idTarefa={id}&origin=DashboardTarefas");
        }

    }
}
