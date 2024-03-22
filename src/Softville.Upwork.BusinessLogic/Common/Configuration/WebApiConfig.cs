// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Softville.Upwork.BusinessLogic.Common.Configuration;

public class WebApiConfig
{
    /// <summary>
    /// Config Section Name
    /// </summary>
    public const string Name = "PrpWebApi";

    public CorsConfig Cors { get; set; } = new();
    public DbConfig Database { get; set; } = new();
}
