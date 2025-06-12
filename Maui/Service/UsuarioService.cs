﻿using Maui.DTOs;
using Maui.DTOs.Usuario;
using Maui.Interface;
using Newtonsoft.Json;
using System.Text;


namespace Maui.Service
{
    class UsuarioService : IUsuarioService
    {
        private readonly HttpClient http;

        public UsuarioService(HttpClient http)
        {
            http = http;
        }

        public async Task<(bool success, string errorMessage)> LoginAsync(UsuarioLoginDTO _dadosLogin)
        {
            try
            {
                var json = JsonConvert.SerializeObject(_dadosLogin);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var endpoint = SetandoEndPoint("api/usuarios/login");

                using (var request = new HttpRequestMessage(HttpMethod.Post, endpoint))
                {
                    request.Content = content;
                    var response = await http.SendAsync(request);

                    var responseContent = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        var result = JsonConvert.DeserializeObject<UserToken>(responseContent);

                        Preferences.Set("authToken", result.Token);
                        Preferences.Set("usuarioLogado", _dadosLogin.login);

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

        public async Task<(bool success, string errorMessage)> CadastrarUsuarioAsync(UsuarioCadastrarDTO _dadosUsuario)
        {
            try
            {
                var json = JsonConvert.SerializeObject(_dadosUsuario);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var endpoint = SetandoEndPoint("api/usuarios");

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

        #region Metodo Privado

        private string SetandoEndPoint(string _endpont)
        {
#if DEBUG
            return $"{_endpont}";

#else
            return $"https://blazor-api.onrender.com/{_endpont}";

#endif
        }

        #endregion Metodo Privado
    }
}
