﻿// See https://aka.ms/new-console-template for more information


using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Prospecting.WebJob.Common;
using Softville.Upwork.BusinessLogic.Processor.Migrator;

using IHost host = CreateHostBuilder(args).Build();
await host.StartAsync();

CancellationToken ct = CancellationToken.None;

// await host.Services.GetRequiredService<IUpworkProcessor>().ProcessOffersAsync(ct);
await host.Services.GetRequiredService<IOffersMigrator>().MigrateAsync(ct);

// Console.WriteLine("Press any key to continue");
// Console.ReadKey();

static IHostBuilder CreateHostBuilder(string[] args)
{
    return WebJobHostBuilder.CreateBuilder(args)
        .UseConsoleLifetime();
}
