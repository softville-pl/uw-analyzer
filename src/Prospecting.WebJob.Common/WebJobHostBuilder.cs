using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Softville.Upwork.BusinessLogic;

namespace Prospecting.WebJob.Common;

public static class WebJobHostBuilder
{
    public static IHostBuilder CreateBuilder(string[] args, bool isTextContext = false)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, configBuilder) =>
            {
                configBuilder
                    .AddJsonFile("appSettings.json", isTextContext)
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
}
