
using Maui.Interface;
using Maui.ViewModel.Usuario;

namespace Maui.View;

public partial class LoginPage : ContentPage
{
    public LoginPage(IUsuarioService _usuarioService, IServiceProvider _services)
    {
        InitializeComponent();

        var navigation = this.Navigation;

        BindingContext = new LoginViewModel(_usuarioService, _services, navigation);
    }
}