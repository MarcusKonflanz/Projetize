using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using Projetize.App;
using Projetize.App.Services;
using Blazored.LocalStorage;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Componentes raiz da aplica��o
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Servi�os de UI e componentes (MudBlazor)
builder.Services.AddMudServices();

// Servi�os personalizados e utilit�rios
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddBlazoredLocalStorage();

// HttpClient configurado para comunica��o com a API
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://localhost:7089")
});

await builder.Build().RunAsync();
