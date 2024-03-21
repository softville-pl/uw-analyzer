// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Softville.Upwork.BusinessLogic.Common.Configuration;

public class CorsConfig
{
    /// <summary>
    /// Config Section Name
    /// </summary>
    public const string Name = "Cors";

    public string[] CorsOrigins { get; set; } = { };
}
