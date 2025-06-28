using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Maui.DTOs.Usuario;
using Maui.Interface;
using Maui.View.Tarefa;
using Maui.View.Usuario;
using System.ComponentModel.DataAnnotations;

namespace Maui.ViewModel.Usuario
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly IUsuarioService iUsuarioService;
        private readonly IServiceProvider services;
        private readonly INavigation navigation;

        public LoginViewModel(IUsuarioService _iUsuarioService, IServiceProvider _services, INavigation _navigation)
        {
            iUsuarioService = _iUsuarioService;
            services = _services;
            navigation = _navigation;

            UsuarioLogin = new UsuarioLoginDTO();
        }

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
                var (success, errorMessage) = await iUsuarioService.LoginAsync(usuarioLogin);

                if (success)
                {
                    await Shell.Current.GoToAsync("//DashboardTarefas");


                    //var page = services.GetRequiredService<DashboardTarefas>();

                    //await navigation.PushAsync(page);


                }
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

            //var page = services.GetRequiredService<CadastrarUsuario>();
            //await navigation.PushAsync(page);
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
