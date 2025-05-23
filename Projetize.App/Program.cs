using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using Projetize.App;
using Projetize.App.Services;
using Blazored.LocalStorage;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// declarando MudBlazor
builder.Services.AddMudServices();

// Injeção de Serviços
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddBlazoredLocalStorage();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7089") });

await builder.Build().RunAsync();
