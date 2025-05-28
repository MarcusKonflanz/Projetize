using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using MudBlazor.Services;
using Projetize.App;
using Projetize.App.Services.Auth;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Componentes raiz da aplica��o
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Servi�os de UI e componentes (MudBlazor)
builder.Services.AddMudServices();

// Handlers e HTTP Client
builder.Services.AddScoped<AuthMessageHandler>();

builder.Services.AddHttpClient("AuthorizedClient", client =>
{
    client.BaseAddress = new Uri("https://localhost:7089/");
})
.AddHttpMessageHandler<AuthMessageHandler>();

// Servi�o de autentica��o
builder.Services.AddScoped<IAuthService>(sp =>
{
    var factory = sp.GetRequiredService<IHttpClientFactory>();
    var client = factory.CreateClient("AuthorizedClient");
    var jsRuntime = sp.GetRequiredService<IJSRuntime>();
    return new AuthService(client, jsRuntime);
});

await builder.Build().RunAsync();
