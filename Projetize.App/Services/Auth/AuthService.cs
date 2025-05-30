using Microsoft.JSInterop;
using Projetize.App.Helpers.Utils;
using Projetize.App.Models.Login;
using System.Net.Http.Json;

namespace Projetize.App.Services.Auth
{
    public interface IAuthService
    {
        Task<(bool Succes, string Message)> LoginAsync(LoginModel loginModel);
        Task<bool> IsAuthenticated();
        Task<bool> RefreshTokenAsync();
        Task<(bool Succes, string Message)> RegisterAsync(RegisterModel register);
        Task<(bool Succes, string Message)> ConfirmEmailAsync(ConfirmEmailModel confirmEmail);
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
                var result = await response.Content.ReadFromJsonAsync<AuthResponseModel>();

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
        public async Task<bool> RefreshTokenAsync()
        {
            var token = await jsRuntime.InvokeAsync<string>("localStorage.getItem", "refreshToken");

            if (string.IsNullOrWhiteSpace(token))
                return false;

            var refreshTokenRequest = new RefreshTokenRequestModel { RefreshToken = token };

            var response = await httpClient.PostAsJsonAsync("api/Users/refresh", refreshTokenRequest);

            if (!response.IsSuccessStatusCode)
                return false;

            var result = await response.Content.ReadFromJsonAsync<AuthResponseModel>();
            if (result == null)
                return false;

            await jsRuntime.InvokeVoidAsync("localStorage.setItem", "accessToken", result.Token);
            await jsRuntime.InvokeVoidAsync("localStorage.setItem", "refreshToken", result.RefreshToken);

            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", result.Token);

            return true;
        }
        public async Task<bool> IsAuthenticated()
        {
            var token = await jsRuntime.InvokeAsync<string>("localStorage.getItem", "accessToken");

            if (string.IsNullOrWhiteSpace(token))
                return false;

            try
            {
                var expiration = JwtHelper.GetExpiration(token);
                if (expiration == null || expiration <= DateTime.UtcNow)
                    return false;
            }
            catch
            {
                return false;
            }

            return true;
        }
        public async Task<(bool Succes, string Message)> RegisterAsync(RegisterModel registerModel)
        {
            var response = await httpClient.PostAsJsonAsync("api/Users/register", registerModel);
            var responseContent = await response.Content.ReadAsStringAsync();

            return response.IsSuccessStatusCode
                ? (true, "Cadastro realizado com sucesso.")
                : (false, responseContent);
        }
        public async Task<(bool Succes, string Message)> ConfirmEmailAsync(ConfirmEmailModel confirmEmail)
        {
            var response = await httpClient.PostAsJsonAsync("api/Users/confirm-email", confirmEmail);
            var responseContent = await response.Content.ReadAsStringAsync();

            return response.IsSuccessStatusCode
                ? (true, "E-mail confirmado com sucesso")
                : (false, responseContent);
        }
    }
}
