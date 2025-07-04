using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maui.DTOs.Tarefa;
using Maui.Interface;
using Maui.Models;

namespace Maui.ViewModel.Tarefa
{

    [QueryProperty(nameof(IdTarefa), "idTarefa")]

    [QueryProperty(nameof(Origin), "origin")]
    public partial class EditarTarefaViewModel : ObservableObject
    {
        private readonly ITarefaService iTarefaService;

        private readonly INotificacaoService iNotificacaoService;

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
        private bool isEnabled = true;

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
        private bool mostarBtnVoltar;

        [ObservableProperty]
        private List<ItemCabecalhoTarefa.ItensCabecario> listaItensCabecalho = new List<ItemCabecalhoTarefa.ItensCabecario>();

        [ObservableProperty]
        private List<string> listaDePrioridades = new List<string>();

        [ObservableProperty]
        private List<string> listaDeStatus = new List<string>();


        public EditarTarefaViewModel(ITarefaService _iTarefaService, INotificacaoService _iNotificacaoService)
        {
            iTarefaService = _iTarefaService;

            iNotificacaoService = _iNotificacaoService;

            TarefaAlterarDTO = new TarefaAlterarDTO();

            PreencherListaPrioridade();

            PreencherListaStatus();

            PreencherCabecario();


            MostrarErroTitulo = false;
            MostrarErroPrioridade = false;
            MostrarErroPrazo = false;
            MostrarErroDescricao = false;
            MostrarErroStatus = false;
        }

        partial void OnIdTarefaChanged(int value)
        {
            CarregarTarefaAsync(value);
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
        private void PreencherCabecario()
        {
            IconeCabecalho = "lapis.png";
            TituloCabecalho = "Editar Tarefa";
            SubtituloCabecalho = "Atualize os detalhes da sua tarefa para manter sua produtividade em dia.";

            PreencherListaItensCabecario();
        }

        private void PreencherListaItensCabecario()
        {

            ListaItensCabecalho.AddRange(new[]
            {
                new ItemCabecalhoTarefa.ItensCabecario
                {
                    Icone = "aceitar.png",
                    Texto = "Organize suas prioridades de forma eficiente."
                },
                new ItemCabecalhoTarefa.ItensCabecario
                {
                    Icone = "aceitar.png",
                    Texto = "Acompanhe prazos e progressos."
                },
                new ItemCabecalhoTarefa.ItensCabecario
                {
                    Icone = "aceitar.png",
                    Texto = "Matenha o seu fluxo de trabalho organizado."
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


        [RelayCommand]
        private async Task Salvar()
        {
            if (!IsCamposPreenchidos())
            {
                return;
            }

            await CadastrarTarefaAsync();
        }

        private async Task CadastrarTarefaAsync()
        {
            try
            {
                (bool Sucesso, string ErrorMessagem) = await iTarefaService.AlterarTarefaAsync(TarefaAlterarDTO);

                if (Sucesso)
                {


                    var resposta = await Application.Current.MainPage.DisplayAlert("Atenção!",
                        "Tem certeza de que deseja salvar as alterações feitas nesta tarefa?",
                        "Sim", "Não");

                    if (resposta)
                    {

                    await Shell.Current.GoToAsync($"//{Origin}");

                    await iNotificacaoService.MostrarNotificacaoAsync("Operação realizada com sucesso!");
                    }

                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Atenção!", ErrorMessagem, "OK");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao realizar Edição da tarefa: {ex.Message}");

                await Application.Current.MainPage.DisplayAlert("Atenção!",
                    "Ocorreu um erro interno. Nossa equipe já foi notificada.", "OK");
            }
        }

        private bool IsCamposPreenchidos()
        {
            MostrarErroTitulo = string.IsNullOrEmpty(TarefaAlterarDTO?.Titulo) ? true : false;
            MostrarErroPrioridade = string.IsNullOrEmpty(TarefaAlterarDTO?.Prioridade) ? true : false;
            MostrarErroPrazo = string.IsNullOrEmpty(TarefaAlterarDTO?.Prazo?.ToString()) ? true : false;
            MostrarErroDescricao = string.IsNullOrEmpty(TarefaAlterarDTO?.Descricao) ? true : false;
            MostrarErroStatus = string.IsNullOrEmpty(TarefaAlterarDTO?.Status) ? true : false;

            OnPropertyChanged(nameof(MostrarErroTitulo));
            OnPropertyChanged(nameof(MostrarErroPrioridade));
            OnPropertyChanged(nameof(MostrarErroPrazo));
            OnPropertyChanged(nameof(MostrarErroDescricao));
            OnPropertyChanged(nameof(MostrarErroStatus));

            if (MostrarErroTitulo || MostrarErroPrioridade || MostrarErroPrazo || MostrarErroDescricao || MostrarErroStatus)
            {
                return false;
            }

            return true;
        }



        [RelayCommand]
        private async Task Cancelar()
        {
            var resposta = await Application.Current.MainPage.DisplayAlert("Atenção!",
                "Tem certeza que deseja cancelar a edição do cadastro desta tarefa?",
                "Sim", "Não");

            if (resposta)
            {

                await Shell.Current.GoToAsync($"//{Origin}");
            }
        }
    }
}
