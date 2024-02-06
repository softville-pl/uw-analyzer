// See https://aka.ms/new-console-template for more information


using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Softville.Upwork.BusinessLogic;
using Softville.Upwork.BusinessLogic.Processor;

using IHost host = CreateHostBuilder(args).Build();
await host.StartAsync();

CancellationToken ct = CancellationToken.None;

await host.Services.GetRequiredService<IUpworkProvider>().ProvideOffers(ct);

// await host.Services.GetRequiredService<IUpworkProcessor>().ProcessOffersAsync(ct);

Console.WriteLine("Press any key to continue");
Console.ReadKey();

static IHostBuilder CreateHostBuilder(string[] args)
{
    return Host.CreateDefaultBuilder(args)
        .UseConsoleLifetime()
        .ConfigureAppConfiguration((context, configBuilder) =>
        {
            configBuilder
                .AddJsonFile("appSettings.json", false)
                .AddJsonFile($"appSettings.{context.HostingEnvironment.EnvironmentName}.json", true)
                .AddEnvironmentVariables(
                    "UPWORK_ANALYZER:"); //https://learn.microsoft.com/en-us/dotnet/core/compatibility/extensions/7.0/environment-variable-prefixn
        })
        .ConfigureLogging((_, builder) => builder
            .AddConsole()
            .AddFilter("Microsoft.Hosting", LogLevel.Warning)
            .AddFilter("System.Net.Http", LogLevel.Warning)
            .SetMinimumLevel(LogLevel.Information))
        .ConfigureServices((context, services) => { services.AddBusinessLogic(context.Configuration); });
}
