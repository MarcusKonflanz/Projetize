using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Projetize.App.Helpers.Utils;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Projetize.App.Services.Auth
{
    public class AuthMessageHandler : DelegatingHandler
    {
        private readonly IJSRuntime jsRuntime;
        private readonly NavigationManager navigationManager;
        private readonly IAuthService authService;

        public AuthMessageHandler(IJSRuntime jsRuntime, NavigationManager navigationManager, IAuthService authService)
        {
            this.jsRuntime = jsRuntime;
            this.navigationManager = navigationManager;
            this.authService = authService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await jsRuntime.InvokeAsync<string>("localStorage.getItem", "accessToken");

            if (!string.IsNullOrWhiteSpace(token))
            {
                var expiration = JwtHelper.GetExpiration(token);

                if (expiration != null && expiration <= DateTime.UtcNow.AddSeconds(30))
                {
                    var refreshed = await authService.RefreshTokenAsync();
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
    }
}
