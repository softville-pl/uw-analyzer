// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Prospecting.WebJob.Common;
using Softville.Upwork.BusinessLogic.Processor.UpworkApi;
using Softville.Upwork.Tests.Common.Stubs;

namespace Softville.Upwork.BusinessLogic.IntTests.Infrastructure;

public sealed class TestWebJob : IAsyncLifetime, IDisposable
{
    private TestServices? _services ;

    private readonly IHostBuilder _hostBuilder = WebJobHostBuilder.CreateBuilder([], isTextContext: true);
    private IHost? _host;

    public List<KeyValuePair<string, string?>> Configuration { get; } = new();

    public TestServices Services { get => _services ?? throw new ArgumentNullException(nameof(_services)); }
    public async Task InitializeAsync()
    {
        var ct = CancellationToken.None;

        _hostBuilder.ConfigureAppConfiguration((_, builder) =>
        {
            builder.AddInMemoryCollection(Configuration);
        });
        _hostBuilder.ConfigureServices((_, services) =>
            {
                services.RemoveAll(typeof(IHttpResponsePersisting));
                services.AddScoped<IHttpResponsePersisting, InMemoryPersistingStub>();
            }
        );
        _host = await _hostBuilder.StartAsync(ct);
        _services = new(_host?.Services ?? throw new ArgumentNullException(nameof(_host)));
    }

    public async Task DisposeAsync() => await (_host?.StopAsync() ?? Task.CompletedTask);

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (disposing)
        {
            _services?.Dispose();
            _services = null;
            _host?.Dispose();
            _host = null;
        }
    }

    public async Task ResetAsync()
    {
        Services.InitScope();
        await Task.CompletedTask;
    }
}
