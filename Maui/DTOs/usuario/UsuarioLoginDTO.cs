using CommunityToolkit.Mvvm.ComponentModel;

namespace Maui.DTOs.Usuario
{
    public class UsuarioLoginDTO : ObservableValidator
    {
        public string Login { get; set; }

        public string Senha { get; set; }
    }
}