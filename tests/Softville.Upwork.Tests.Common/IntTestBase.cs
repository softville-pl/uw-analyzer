// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Softville.Upwork.Tests.Common;

public abstract class IntTestBase<TContext>(TContext ctx) : IAsyncLifetime where TContext : ITestContext
{
    protected TContext Ctx { get; } = ctx;

    public async Task InitializeAsync() => await Ctx.StartTestAsync();

    public async Task DisposeAsync() => await Ctx.StopTestAsync();
}
