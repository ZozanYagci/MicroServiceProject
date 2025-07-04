using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using WebApp;
using WebApp.Application.Services;
using WebApp.Application.Services.Interfaces;
using WebApp.Infrastructure;
using WebApp.Utils;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Root components
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Services
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddBlazoredLocalStorage();

builder.Services.AddTransient<IIdentityService, IdentityService>();
builder.Services.AddTransient<ICatalogService, CatalogService>();
builder.Services.AddTransient<IBasketService, BasketService>();
builder.Services.AddTransient<IOrderService, OrderService>();

builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();

builder.Services.AddSingleton<AppStateManager>();

builder.Services.AddScoped(sp =>
{
    var clientFactory = sp.GetRequiredService<IHttpClientFactory>();
    return clientFactory.CreateClient("ApiGatewayHttpClient");
});


builder.Services.AddHttpClient("ApiGatewayHttpClient", client =>
{
    client.BaseAddress = new Uri("https://localhost:5000/");
});

// Run
await builder.Build().RunAsync();
