﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Softville.Upwork.WebApp.Configuration;

public class WebAppConfig
{
    /// <summary>
    /// Config Section Name
    /// </summary>
    public const string Name = "PrpWebApp";

    public required BackendConfig Backend { get; set; }
}
