// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Softville.Upwork.BusinessLogic.IntTests.Infrastructure;

public abstract class IntTestBase
{
    public IntPrpContext Ctx { get; }

    public IntTestBase(IntPrpContext ctx)
    {
        Ctx = ctx;
    }
}
