using Maui.DTOs.Usuario;

namespace Maui.Interface
{
    public interface IUsuarioService
    {
        Task<(bool Sucesso, string ErrorMessagem)> LoginAsync(UsuarioLoginDTO _dadosLogin);

        Task<(bool Sucesso, string ErrorMessagem)> CadastrarUsuarioAsync(UsuarioCadastrarDTO _dadosUsuario);
    }
}