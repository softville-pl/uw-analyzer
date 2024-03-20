// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Softville.Upwork.Tests.Common.Components;

namespace Softville.Upwork.BusinessLogic.IntTests.Infrastructure;

public class IntPrpContext : IAsyncLifetime, IDisposable
{
    public DatabaseComponent Database { get; } = new();
    public InternetComponent NetProxy { get; } = new();
    public WebJobComponent Job { get; } = new();
    public CancellationToken Ct { get; } = CancellationToken.None;

    public async Task InitializeAsync()
    {
        var ct = CancellationToken.None;

        await Database.StartAsync(ct);

        Job.Configuration.AddRange(
        [
            new KeyValuePair<string, string?>("Database:ConnectionString", Database.ConnectionString),
            new KeyValuePair<string, string?>("Upwork:BaseUrl", NetProxy.Url),
            new KeyValuePair<string, string?>("Upwork:Cookie", Guid.NewGuid().ToString())
        ]);

        await Job.InitializeAsync();
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
