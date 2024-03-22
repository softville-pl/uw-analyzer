// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Softville.Upwork.Tests.Common;

public interface ITestContext
{
    public Task StartTestAsync();

    public Task StopTestAsync();

}
