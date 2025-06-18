using Maui.Interface;
using Maui.ViewModel.Usuario;

namespace Maui.View.Usuario;

public partial class CadastrarUsuario : ContentPage
{
    public CadastrarUsuario(IUsuarioService _usuarioService, IServiceProvider _services)
    {
        InitializeComponent();

        var navigation = this.Navigation;

        BindingContext = new CadastrarUsuarioViewModel(_usuarioService, _services, navigation);
    }
}