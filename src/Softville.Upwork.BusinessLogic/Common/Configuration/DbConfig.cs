﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Softville.Upwork.BusinessLogic.Common.Configuration;

public class DbConfig
{
    public required string ConnectionString { get; set; }
    public required string DatabaseName { get; set; } = "Prospecting";
}