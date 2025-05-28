using Docker.DotNet.Models;
using Microsoft.JSInterop;
using Projetize.App.Helpers.Utils;
using Projetize.App.Models.Login;
using System.Net.Http.Json;

namespace Projetize.App.Services.Auth
{
    public interface IAuthService
    {
        Task<(bool Succes, string Message)> LoginAsync(LoginModel loginModel);
    }
    public class AuthService : IAuthService
    {
        private readonly HttpClient httpClient;
        private readonly IJSRuntime jsRuntime;

        public AuthService(HttpClient httpClient, IJSRuntime jsRuntime)
        {
            this.httpClient = httpClient;
            this.jsRuntime = jsRuntime;
        }

        public async Task<(bool Succes, string Message)> LoginAsync(LoginModel loginModel)
        {
            var response = await httpClient.PostAsJsonAsync("api/Users/login", loginModel);

            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<Models.Login.AuthResponse>();

                // Armazena os tokens no localStorage
                await jsRuntime.InvokeVoidAsync("localStorage.setItem", "accessToken", result.Token);
                await jsRuntime.InvokeVoidAsync("localStorage.setItem", "refreshToken", result.RefreshToken);

                return (true, "Login realizado com sucesso.");
            }

            return (false, responseContent);
        }

        public async Task<(bool Succes, string Message)> InitializeAsync()
        {
            var token = await jsRuntime.InvokeAsync<string>("localStorage.getItem", "accessToken");

            if (string.IsNullOrWhiteSpace(token))
                return (false, "Token inválido");

            var expiration = JwtHelper.GetExpiration(token);
            if (expiration == null || expiration <= DateTime.UtcNow)
                return (false, "Token expirado.");

            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            return (true, "Token restaurado com sucesso.");
        }

        public async Task<bool Succes> RefreshTokenAsync()
        {
            var token = await jsRuntime.InvokeAsync<string>("localStorage.getItem", "refreshToken");

            var response = await httpClient.PostAsJsonAsync("api/Users/refresh", token);

            if (response == null)
                return false;

            if (response.IsSuccessStatusCode)
            {

            }

        }

    }
}