using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Maui.DTOs.Usuario;
using Maui.Interface;
using System.Net.Sockets;

namespace Maui.ViewModel.Usuario
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly IUsuarioService iUsuarioService;

        [ObservableProperty]
        private UsuarioLoginDTO usuarioLogin;

        [ObservableProperty]
        private bool mostrarErroSenha;

        [ObservableProperty]
        private bool mostrarErroUsuario;

        [ObservableProperty]
        private bool isRefreshing;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IconeOlhoSenha))]
        private bool isSenhaOculta = true;

        public LoginViewModel(IUsuarioService _iUsuarioService)
        {
            iUsuarioService = _iUsuarioService;

            UsuarioLogin = new UsuarioLoginDTO();
        }

        [RelayCommand]
        private async Task Entrar()
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

            await RealizarLoginAsync();
        }

        private async Task RealizarLoginAsync()
        {
            try
            {
                var (Sucesso, ErrorMessagem) = await iUsuarioService.LoginAsync(usuarioLogin);

                if (Sucesso)
                {
                    await Shell.Current.GoToAsync("//DashboardTarefas");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Atenção!", $"{ErrorMessagem}", "OK");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao realizar login: {ex.Message}");

                if (ex is HttpRequestException || ex.InnerException is SocketException)
                {
                    await Application.Current.MainPage.DisplayAlert("Atenção!",
                        "Não foi possível conectar. Verifique sua conexão com a internet.", "OK");
                }
                else if (ex is TaskCanceledException)
                {
                    await Application.Current.MainPage.DisplayAlert("Atenção!",
                        "A solicitação demorou demais para responder. Verifique sua conexão.", "OK");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Atenção!",
                        "Ocorreu um erro interno. Nossa equipe já foi notificada.", "OK");
                }
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        private bool IsCamposPreenchidos()
        {
            MostrarErroUsuario = string.IsNullOrEmpty(UsuarioLogin?.Login) ? true : false;
            MostrarErroSenha = string.IsNullOrEmpty(UsuarioLogin?.Senha) ? true : false;

            OnPropertyChanged(nameof(MostrarErroUsuario));
            OnPropertyChanged(nameof(MostrarErroSenha));

            if (MostrarErroUsuario || MostrarErroSenha)
            {
                return false;
            }

            return true;
        }

        [RelayCommand]
        public async void CriarConta()
        {
            await Shell.Current.GoToAsync("//CadastrarUsuario");
        }

        public string IconeOlhoSenha => IsSenhaOculta ? "olho_aberto.png" : "olho_fechado.png";

        [RelayCommand]
        private void ExibirSenha()
        {
            IsSenhaOculta = !IsSenhaOculta;
            OnPropertyChanged(nameof(IconeOlhoSenha));
        }

        ////public async Task PegarUsuarioLogadoAsync()
        ////{
        ////    string usuarioLogado = await jsRuntime.InvokeAsync<string>("localStorage.getItem", "usuario");

        ////    if (!string.IsNullOrEmpty(usuarioLogado))
        ////    {
        ////        usuarioLogin.login = usuarioLogado.Replace("\"", "");

        ////    }
        ////}
    }
}