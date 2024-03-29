﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Softville.Upwork.BusinessLogic.Common.Configuration;

public class UpworkConfig
{
    /// <summary>
    /// Config Section Name
    /// </summary>
    public const string Name = "Upwork";

    public required string Cookie { get; set; }
    public required string BaseUrl { get; set; }
}
