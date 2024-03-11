// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Xunit;

namespace Softville.Upwork.BusinessLogic.IntTests.Infrastructure;

public abstract class IntTestBase(IntPrpContext ctx) : IAsyncLifetime
{
    public IntPrpContext Ctx { get; } = ctx;

    public async Task InitializeAsync() => await Ctx.StartTestAsync();

    public async Task DisposeAsync() => await Ctx.StopTestAsync();
}
