using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Maui.DTOs.Tarefa;
using Maui.Interface;

namespace Maui.ViewModel.Tarefa
{
    public partial class CadastrarTarefaViewModel : ObservableObject
    {
        private readonly ITarefaService iTarefaService;

        private readonly INotificacaoService iNotificacaoService;

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
        private TarefaAlterarDTO tarefaAlterarDTO;

        [ObservableProperty]
        private List<string> listaDePrioridades = new List<string>();

        [ObservableProperty]
        private List<string> listaDeStatus = new List<string>();


        public CadastrarTarefaViewModel(ITarefaService _iTarefaService, INotificacaoService _iNotificacaoService)
        {
            iTarefaService = _iTarefaService;

            TarefaAlterarDTO = new TarefaAlterarDTO();

            PreencherListaPrioridade();

            PreencherListaStatus();

            iNotificacaoService = _iNotificacaoService;
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
                (bool Sucesso, string ErrorMessagem) = await iTarefaService.CadastrarTarefaAsync(TarefaAlterarDTO);

                if (Sucesso)
                {

                    await iNotificacaoService.MostrarNotificacaoAsync("Operação realizada com sucesso!");

                    LimparCampos();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Atenção!", ErrorMessagem, "OK");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao realizar Cadastro da tarefa: {ex.Message}");

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

            if (MostrarErroTitulo || MostrarErroPrioridade || MostrarErroPrazo || MostrarErroDescricao ||
                MostrarErroStatus)
            {
                return false;
            }

            return true;
        }

        private void LimparCampos()
        {
            TarefaAlterarDTO = new TarefaAlterarDTO();

            OnPropertyChanged(nameof(TarefaAlterarDTO));
        }

        [RelayCommand]
        private async Task Cancelar()
        {
            var resposta = await Application.Current.MainPage.DisplayAlert("Atenção!",
                "Tem certeza que deseja cancelar o cadastro da tarefa?",
                "Sim", "Não");

            if (resposta)
            {
                await Shell.Current.GoToAsync("//DashboardTarefas");
            }
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

    }
}
