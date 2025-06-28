
using Maui.Interface;
using Maui.ViewModel.Usuario;

namespace Maui.View.Usuario;

public partial class Login : ContentPage
{
    public Login(IUsuarioService _usuarioService, IServiceProvider _services)
    {
        InitializeComponent();

        var navigation = this.Navigation;

        BindingContext = new LoginViewModel(_usuarioService, _services, navigation);
    }
}