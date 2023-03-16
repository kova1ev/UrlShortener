using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using UrlShortener.ClientWasm;
using UrlShortener.ClientWasm.ApiHttpClient;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddHttpClient<ILinkHttpClient, LinkHttpClient>(client =>
    //  client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
    client.BaseAddress = new Uri("https://localhost:7072")
    );


await builder.Build().RunAsync();
