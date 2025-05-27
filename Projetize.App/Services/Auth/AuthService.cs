using Docker.DotNet.Models;
using Microsoft.JSInterop;
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

        public AuthService(HttpClient httpClient, IJSRuntime jsRuntime  )
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
    }
}