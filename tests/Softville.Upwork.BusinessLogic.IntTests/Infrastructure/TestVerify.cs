// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Softville.Upwork.BusinessLogic.IntTests.Infrastructure;

public class TestVerify
{
    internal VerifySettings CreateSettings()
    {
        var result = new VerifySettings();
        result.DontScrubDateTimes();

        return result;
    }
}
