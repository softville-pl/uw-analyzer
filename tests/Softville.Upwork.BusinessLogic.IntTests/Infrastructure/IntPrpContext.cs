// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Prospecting.WebJob.Common;
using Xunit;

namespace Softville.Upwork.BusinessLogic.IntTests.Infrastructure;

public class IntPrpContext : IAsyncLifetime, IDisposable
{
    private readonly IHostBuilder _hostBuilder = WebJobHostBuilder.CreateBuilder([], isTextContext: true);
    private readonly TestDatabase _database = new();
    private IHost? _host;

    private TestServices? _services ;

    public TestServices Services
    {
        get => _services ?? throw new ArgumentNullException(nameof(_services));
    }

    public TestDatabase Database => _database;

    public async Task InitializeAsync()
    {
        var ct = CancellationToken.None;

        await _database.StartAsync(ct);
        _hostBuilder.ConfigureAppConfiguration((_, builder) => builder.AddInMemoryCollection(new[]
        {
            new KeyValuePair<string, string?>("Database:ConnectionString", Database.ConnectionString)
        }));
        _host = await _hostBuilder.StartAsync(ct);
        _services = new(_host?.Services ?? throw new ArgumentNullException(nameof(_host)));
    }

    public async Task DisposeAsync() => await (_host?.StopAsync() ?? Task.CompletedTask);

    public async Task StartTestAsync()
    {
        Services.InitScope();
        await Task.CompletedTask;
    }

    public async Task StopTestAsync() => await Task.CompletedTask;

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _services?.Dispose();
            _services = null;
        }
    }
}
