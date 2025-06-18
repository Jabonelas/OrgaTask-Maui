using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Maui.DTOs.Usuario;
using Maui.Interface;
using Maui.View.Tarefa;

namespace Maui.ViewModel.Usuario
{
    public partial class CadastrarUsuarioViewModel : ObservableObject
    {
        private readonly IUsuarioService iUsuarioService;
        private readonly IServiceProvider services;
        private readonly INavigation navigation;

        public CadastrarUsuarioViewModel(IUsuarioService _iUsuarioService, IServiceProvider _services,
            INavigation _navigation)
        {
            iUsuarioService = _iUsuarioService;
            services = _services;
            navigation = _navigation;

            DadosUsuario = new UsuarioCadastrarDTO();
        }

        [ObservableProperty]
        private UsuarioCadastrarDTO dadosUsuario;

        [ObservableProperty]
        private bool mostrarErroNomeCompleto;

        [ObservableProperty]
        private bool mostrarErroUsuario;

        [ObservableProperty]
        private bool mostrarErroSenha;

        [ObservableProperty]
        private bool mostrarErroConfirmarSenha;
        
        [ObservableProperty]
        private bool isRefreshing;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IconeOlhoSenha))]
        private bool isSenhaOculta = true;
        
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IconeOlhoConfirmacaoSenha))]
        private bool isConfirmacaoSenhaOculta = true;



        [RelayCommand]
        private async Task CadastrarUsuario()
        {

            if (!IsCamposPreenchidos())
            {
                return;
            }

            if (IsRefreshing)
            {
                return;
            }

            IsRefreshing = true;

            await RealizarCadastroAsync();
        }

        private bool IsCamposPreenchidos()
        {
            MostrarErroNomeCompleto = string.IsNullOrEmpty(dadosUsuario?.Nome) ? true : false;
            MostrarErroUsuario = string.IsNullOrEmpty(dadosUsuario?.Login) ? true : false;
            MostrarErroSenha = string.IsNullOrEmpty(dadosUsuario?.Senha) ? true : false;
            MostrarErroConfirmarSenha = string.IsNullOrEmpty(dadosUsuario?.ConfirmarSenha) ? true : false;

            OnPropertyChanged(nameof(MostrarErroNomeCompleto));
            OnPropertyChanged(nameof(MostrarErroUsuario));
            OnPropertyChanged(nameof(MostrarErroSenha));
            OnPropertyChanged(nameof(MostrarErroConfirmarSenha));

            if (MostrarErroNomeCompleto || MostrarErroUsuario || MostrarErroSenha || MostrarErroConfirmarSenha)
            {
                return false;
            }

            return true;

        }


        private async Task RealizarCadastroAsync()
        {
            try
            {
                (bool success, string errorMessage) = await iUsuarioService.CadastrarUsuarioAsync(dadosUsuario);

                if (success)
                {
                    await RealizarLoginAsync();

                    return;
                }

                Application.Current.MainPage.DisplayAlert("Atenção!", errorMessage, "OK");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao realizar Cadastro do usuário: {ex.Message}");

                Application.Current.MainPage.DisplayAlert("Atenção!",
                    "Ocorreu um erro interno. Nossa equipe já foi notificada.", "OK");
            }
            finally
            {
                IsRefreshing = false;
            }

        }


        private async Task RealizarLoginAsync()
        {
            try
            {
                UsuarioLoginDTO usuarioLogin = new UsuarioLoginDTO
                {
                    Login = dadosUsuario.Login,
                    Senha = dadosUsuario.Senha
                };

                var (success, errorMessage) = await iUsuarioService.LoginAsync(usuarioLogin);

                if (success)
                {
                    var page = services.GetRequiredService<DashboardTarefas>();

                    await navigation.PushAsync(page);
                }

                Application.Current.MainPage.DisplayAlert("Atenção!", errorMessage, "OK");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao realizar login: {ex.Message}");

                Application.Current.MainPage.DisplayAlert("Atenção!",
                    "Ocorreu um erro interno. Nossa equipe já foi notificada.", "OK");
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        public string IconeOlhoSenha => IsSenhaOculta ? "olho_aberto.png" : "olho_fechado.png";
        public string IconeOlhoConfirmacaoSenha => IsConfirmacaoSenhaOculta ? "olho_aberto.png" : "olho_fechado.png";

        [RelayCommand]
        private void ExibirSenha()
        {
            IsSenhaOculta = !IsSenhaOculta;
            OnPropertyChanged(nameof(IconeOlhoSenha));
        }

        [RelayCommand]
        private void ExibirConfirmacaoSenha()
        {
            IsConfirmacaoSenhaOculta = !IsConfirmacaoSenhaOculta;
            OnPropertyChanged(nameof(IconeOlhoConfirmacaoSenha));
        }
    }
}
