// See https://aka.ms/new-console-template for more information


using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Softville.Upwork.BusinessLogic;
using Softville.Upwork.BusinessLogic.Processor;

using IHost host = CreateHostBuilder(args).Build();
await host.StartAsync();

var ct = CancellationToken.None;

await host.Services.GetRequiredService<IUpworkProvider>().ProvideOffers(ct);

// IUpworkProcessor processor = host.Services.GetRequiredService<IUpworkProcessor>();
// await processor.ProcessOffersAsync(ct);

Console.WriteLine("Press any key to continue");
Console.ReadKey();

static IHostBuilder CreateHostBuilder(string[] args)
{
    return Host.CreateDefaultBuilder(args)
        .UseConsoleLifetime()
        .ConfigureLogging((_, builder) => builder
            .AddConsole()
            .AddFilter("Microsoft.Hosting", LogLevel.Warning)
            .AddFilter("System.Net.Http", LogLevel.Warning)
            .SetMinimumLevel(LogLevel.Information))
        .ConfigureServices((_, services) => { services.AddBusinessLogic(); });
}
