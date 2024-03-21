// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Softville.Upwork.WebApp.Configuration;

public class BackendConfig
{
    /// <summary>
    /// Config Section Name
    /// </summary>
    public const string Name = "Backend";
    public required string BaseUrl { get; set; }
}
