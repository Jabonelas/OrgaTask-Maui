using Maui.DTOs.Usuario;


namespace Maui.Interface
{
    public interface IUsuarioService
    {
        Task<(bool success, string errorMessage)> LoginAsync(UsuarioLoginDTO _dadosLogin);

        Task<(bool success, string errorMessage)> CadastrarUsuarioAsync(UsuarioCadastrarDTO _dadosUsuario);
    }
}
