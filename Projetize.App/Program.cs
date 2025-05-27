using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using MudBlazor.Services;
using Projetize.App;
using Projetize.App.Services.Auth;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Componentes raiz da aplicação
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Serviços de UI e componentes (MudBlazor)
builder.Services.AddMudServices();

// Serviços personalizados e utilitários
builder.Services.AddScoped<IAuthService>(sp =>
{
    var factory = sp.GetRequiredService<IHttpClientFactory>();
    var client = factory.CreateClient("AuthorizedClient");
    var jsRuntime = sp.GetRequiredService<IJSRuntime>();
    return new AuthService(client, jsRuntime);
});

// HttpClient configurado para comunicação com a API
builder.Services.AddScoped<AuthMessageHandler>();
builder.Services.AddHttpClient("AuthorizedClient", client =>
{
    client.BaseAddress = new Uri("https://localhost:7089/");
})
.AddHttpMessageHandler<AuthMessageHandler>();


await builder.Build().RunAsync();
