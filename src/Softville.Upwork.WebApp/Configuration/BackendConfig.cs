// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Softville.Upwork.WebApp.Configuration;

public class BackendConfig
{
    public const string ConfigSectionName = "Backend";
    public required string BaseUrl { get; set; }
}
