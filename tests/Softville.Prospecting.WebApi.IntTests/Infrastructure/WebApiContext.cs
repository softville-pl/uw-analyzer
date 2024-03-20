﻿using Softville.Upwork.BusinessLogic.Common.Configuration;
using Softville.Upwork.Tests.Common;
using Softville.Upwork.Tests.Common.Components;
using Softville.Upwork.WebApi.IntTests.Infrastructure.Components;

namespace Softville.Upwork.WebApi.IntTests.Infrastructure;

public class WebApiContext : IDisposable, IAsyncLifetime, ITestContext
{
    public DatabaseComponent Database { get; } = new();
    public InternetComponent NetProxy { get; } = new();
    public WebJobComponent Job { get; } = new();

    public WebApiComponent Api { get; } = new();

    public VerifyComponent Verify { get; } = new();

    public CancellationToken Ct { get; } = CancellationToken.None;

    public async Task InitializeAsync()
    {
        var ct = CancellationToken.None;

        await Database.StartAsync(ct);

        var jobTask = Job.StartAsync(new PrpConfig
        {
            Database = new DbConfig {ConnectionString = Database.ConnectionString,},
            Upwork = new UpworkConfig {BaseUrl = NetProxy.Url, Cookie = Guid.NewGuid().ToString()}
        });

        var apiTask = Api.StartAsync(new WebApiConfig
        {
            Database = new DbConfig {ConnectionString = Database.ConnectionString}
        });

        await Task.WhenAll(apiTask, jobTask);
    }

    public async Task DisposeAsync()
    {
        var jobTask = Job.DisposeAsync();
        var apiTask = Api.StopAsync(CancellationToken.None);

       await Task.WhenAll(jobTask, apiTask);
    }

    public async Task StartTestAsync()
    {
        await Database.CleanupDatabaseAsync();
        await Job.ResetAsync();
        NetProxy.Reset();
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
            Job.Dispose();
        }
    }
}
