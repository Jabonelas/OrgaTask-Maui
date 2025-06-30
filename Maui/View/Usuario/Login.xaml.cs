
using Maui.Interface;
using Maui.ViewModel.Usuario;

namespace Maui.View.Usuario;

public partial class Login : ContentPage
{
    public Login(IUsuarioService _usuarioService)
    {
        InitializeComponent();

        BindingContext = new LoginViewModel(_usuarioService);
    }
}