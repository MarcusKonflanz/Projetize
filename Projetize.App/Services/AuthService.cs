using Docker.DotNet.Models;
using Projetize.App.Models.Login;
using System.Net.Http.Json;

namespace Projetize.App.Services
{
    public interface IAuthService
    {
        Task<bool> LoginAsync(LoginModel loginModel);
    }
    public class AuthService : IAuthService
    {
        private readonly HttpClient httpClient;

        public AuthService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<bool> LoginAsync(LoginModel loginModel)
        {
            var response = await httpClient.PostAsJsonAsync("api/Users/login", loginModel);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<Models.Login.AuthResponse>();

                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", result.Token);

                return true;
            }

            return false;
        }

    }
}