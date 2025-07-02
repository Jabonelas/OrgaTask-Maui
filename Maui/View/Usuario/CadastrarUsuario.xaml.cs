using Maui.Interface;
using Maui.ViewModel.Usuario;

namespace Maui.View.Usuario;

public partial class CadastrarUsuario : ContentPage
{
    public CadastrarUsuario(IUsuarioService _usuarioService, INotificacaoService _iNotificacaoService)
    {
        InitializeComponent();

        BindingContext = new CadastrarUsuarioViewModel(_usuarioService, _iNotificacaoService);
    }
}