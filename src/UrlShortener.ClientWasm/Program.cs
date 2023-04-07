using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using UrlShortener.ClientWasm;
using UrlShortener.ClientWasm.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddHttpClient<ILinkHttpService, LinkHttpService>(client =>
     client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
    );
builder.Services.AddHttpClient<IAuthenticationService, AuthenticationService>(client =>
     client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
    );
var webHost = builder.Build();
await webHost.RunAsync();
