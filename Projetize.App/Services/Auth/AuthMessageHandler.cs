using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

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
        public async Task SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await jsRuntime.InvokeAsync<string>("localStorage.getItem", "token");
        }

    }
}
