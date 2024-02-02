// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Softville.Upwork.Contracts;

public class Technology
{
    public required string Architecture { get; set; }
    public required string DotnetVersion { get; set; }
    public required string BackendFrameworks { get; set; }
    public required string FrontendFrameworks { get; set; }
    public required string Databases { get; set; }
    public required Cloud Cloud { get; set; }
    public required string ThirdParty { get; set; }
}

public class Cloud
{
    public required string Provider { get; set; }
    public required string Services { get; set; }
}
