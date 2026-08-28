using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using PPFramWA.Client;
using PPFramWA.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<ApiServicio>();
builder.Services.AddScoped<RecursoServicio>();
builder.Services.AddScoped<ItemServicio>();
builder.Services.AddScoped<JugadorServicio>();
builder.Services.AddScoped<VentaServicio>();
builder.Services.AddScoped<JugadorState>();

await builder.Build().RunAsync();
