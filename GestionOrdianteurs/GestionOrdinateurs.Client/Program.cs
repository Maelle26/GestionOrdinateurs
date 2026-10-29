using Blazored.SessionStorage;
using GestionOrdinateurs.Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// HttpClient pour appeler l'API d'authentification
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5010/") });
builder.Services.AddBlazoredSessionStorage();

builder.Services.AddScoped(sp => new HttpClient 
{ 
    BaseAddress = new Uri("http://localhost:5010/") 
});

await builder.Build().RunAsync();