using Maui.DTOs;
using Maui.DTOs.Usuario;
using Maui.Helpers;
using Maui.Interface;
using Newtonsoft.Json;
using System.Text;

namespace Maui.Service
{
    internal class UsuarioService : IUsuarioService
    {
        private readonly HttpClient http;

        public UsuarioService(HttpClient _http)
        {
            http = _http;

#if DEBUG

            // Ignora validação de certificado
            if (DeviceInfo.Platform == DevicePlatform.Android)
            {
                var handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
                };

                http = new HttpClient(handler);
            }

#endif
        }

        public async Task<(bool Sucesso, string ErrorMessagem)> LoginAsync(UsuarioLoginDTO _dadosLogin)
        {
            try
            {
                var json = JsonConvert.SerializeObject(_dadosLogin);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var endpoint = ApiRoutes.SetandoEndPoint("api/usuarios/login");

                using (var response = await http.PostAsync(endpoint, content))
                {
                    var responseContent = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        var result = JsonConvert.DeserializeObject<UserToken>(responseContent);

                        Preferences.Set("authToken", result.Token);
                        Preferences.Set("usuarioLogado", _dadosLogin.Login);

                        return (true, null);
                    }

                    var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(responseContent);

                    return (false, errorResponse?.message ?? "Erro desconhecido");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro em LoginAsync: {ex.Message}");

                throw;
            }
        }

        public async Task<(bool Sucesso, string ErrorMessagem)> CadastrarUsuarioAsync(UsuarioCadastrarDTO _dadosUsuario)
        {
            try
            {
                var json = JsonConvert.SerializeObject(_dadosUsuario);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var endpoint = ApiRoutes.SetandoEndPoint("api/usuarios");

                using (var request = new HttpRequestMessage(HttpMethod.Post, endpoint))
                {
                    request.Content = content;

                    var response = await http.SendAsync(request);

                    var responseContent = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        return (true, null);
                    }

                    var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(responseContent);

                    return (false, errorResponse?.message ?? "Erro desconhecido");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro em CadastrarUsuarioAsync: {ex.Message}");

                return (false, ex.Message);
            }
        }
    }
}