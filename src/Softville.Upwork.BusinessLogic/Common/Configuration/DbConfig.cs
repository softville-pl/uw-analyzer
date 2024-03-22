// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Softville.Upwork.BusinessLogic.Common.Configuration;

public class DbConfig
{
    /// <summary>
    /// Config Section Name
    /// </summary>
    public const string Name = "Database";

    public string ConnectionString { get; set; } = "Not set";
    public string DatabaseName { get; init; } = "Prospecting";
}
