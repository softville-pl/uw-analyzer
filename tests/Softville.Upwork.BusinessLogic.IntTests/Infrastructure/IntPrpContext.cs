// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Softville.Upwork.BusinessLogic.Common.Configuration;
using Softville.Upwork.Tests.Common;
using Softville.Upwork.Tests.Common.Components;

namespace Softville.Upwork.BusinessLogic.IntTests.Infrastructure;

public class IntPrpContext : IAsyncLifetime, IDisposable, ITestContext
{
    public DatabaseComponent Database { get; } = new();
    public InternetComponent NetProxy { get; } = new();
    public WebJobComponent Job { get; } = new();
    public CancellationToken Ct { get; } = CancellationToken.None;

    public async Task InitializeAsync()
    {
        var ct = CancellationToken.None;

        await Database.StartAsync(ct);

        var config = new WebJobConfig
        {
            Database = new DbConfig
            {
                ConnectionString = Database.ConnectionString,
            },
            Upwork = new UpworkConfig
            {
                BaseUrl = NetProxy.Url,
                Cookie = Guid.NewGuid().ToString()
            }
        };

        await Job.StartAsync(config);
    }

    public async Task DisposeAsync() => await Job.DisposeAsync();

    public VerifyComponent Verify { get; } = new();

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
