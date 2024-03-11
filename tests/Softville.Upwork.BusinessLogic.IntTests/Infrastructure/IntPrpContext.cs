// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.Hosting;
using Prospecting.WebJob.Common;
using Xunit;

namespace Softville.Upwork.BusinessLogic.IntTests.Infrastructure;

public class IntPrpContext : IAsyncLifetime, IDisposable
{
    private readonly IHostBuilder _hostBuilder = WebJobHostBuilder.CreateBuilder([], isTextContext: true);
    private IHost? _host;

    private TestServices? _services ;

    public TestServices Services
    {
        get => _services ?? throw new ArgumentNullException(nameof(_services));
    }

    public async Task InitializeAsync()
    {
        _host = await _hostBuilder.StartAsync();
        _services = new(_host?.Services ?? throw new ArgumentNullException(nameof(_host)));
    }

    public async Task DisposeAsync() => await (_host?.StopAsync() ?? Task.CompletedTask);

    public async Task StartTestAsync()
    {
        Services.InitScope();
        await Task.CompletedTask;
    }

    public async Task StopTestAsync()
    {
        await Task.CompletedTask;
    }

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
