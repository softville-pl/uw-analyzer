// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Softville.Upwork.Tests.Common.Components;

public class VerifyComponent
{
    public VerifySettings CreateSettings()
    {
        var result = new VerifySettings();
        result.DontScrubDateTimes();

        return result;
    }
}
