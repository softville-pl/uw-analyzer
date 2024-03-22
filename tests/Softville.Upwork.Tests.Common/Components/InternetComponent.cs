// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using WireMock.Server;

namespace Softville.Upwork.Tests.Common.Components;

public class InternetComponent
{
    public WireMockServer Server { get; } = WireMockServer.StartWithAdminInterface();
    public string Url => Server.Url!;

    public void Reset()
    {
        Server.Reset();
    }
}
