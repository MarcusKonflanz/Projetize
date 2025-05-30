using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Projetize.App.Helpers.Utils;
using Projetize.App.Models.Login;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Projetize.App.Services.Auth
{
    public class AuthMessageHandler : DelegatingHandler
    {
        private readonly IJSRuntime jsRuntime;
        private readonly NavigationManager navigationManager;

        public AuthMessageHandler(IJSRuntime jsRuntime, NavigationManager navigationManager)
        {
            this.jsRuntime = jsRuntime;
            this.navigationManager = navigationManager;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await jsRuntime.InvokeAsync<string>("localStorage.getItem", "accessToken");

            if (!string.IsNullOrWhiteSpace(token))
            {
                var expiration = JwtHelper.GetExpiration(token);

                if (expiration != null && expiration <= DateTime.UtcNow.AddSeconds(30))
                {
                    bool refreshed = await TryRefreshTokenAsync(request, cancellationToken);
                    if (!refreshed)
                    {
                        navigationManager.NavigateTo("/login", true);
                        return new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
                    }

                    token = await jsRuntime.InvokeAsync<string>("localStorage.getItem", "accessToken");
                }

                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return await base.SendAsync(request, cancellationToken);
        }

        private async Task<bool> TryRefreshTokenAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var refreshToken = await jsRuntime.InvokeAsync<string>("localStorage.getItem", "refreshToken");

            if (string.IsNullOrWhiteSpace(refreshToken))
                return false;

            var refreshRequest = new RefreshTokenRequestModel
            {
                RefreshToken = refreshToken
            };

            using var httpClient = new HttpClient
            {
                BaseAddress = new Uri(request.RequestUri.GetLeftPart(UriPartial.Authority))
            };

            var response = await httpClient.PostAsJsonAsync("api/Users/refresh", refreshRequest, cancellationToken);

            if (!response.IsSuccessStatusCode)
                return false;

            var result = await response.Content.ReadFromJsonAsync<AuthResponseModel>(cancellationToken: cancellationToken);

            if (result == null)
                return false;

            await jsRuntime.InvokeVoidAsync("localStorage.setItem", "accessToken", result.Token);
            await jsRuntime.InvokeVoidAsync("localStorage.setItem", "refreshToken", result.RefreshToken);

            return true;
        }
    }
}
