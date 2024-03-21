using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Options;
using Softville.Upwork.WebApp;
using Softville.Upwork.WebApp.Configuration;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.Configure<WebAppConfig>(builder.Configuration.GetSection(WebAppConfig.Name));
builder.Services.Configure<BackendConfig>(builder.Configuration.GetSection(WebAppConfig.Name).GetSection(BackendConfig.Name));
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(sp.GetRequiredService<IOptions<BackendConfig>>().Value.BaseUrl)
});

await builder.Build().RunAsync();
