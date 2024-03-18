// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using WireMock.Server;

namespace Softville.Upwork.BusinessLogic.IntTests.Infrastructure;

public class InternetProxy
{
    public WireMockServer Server { get; } = WireMockServer.StartWithAdminInterface();
    public string Url => Server.Url!;

    public void Restart()
    {
        Server.Reset();
    }
}
