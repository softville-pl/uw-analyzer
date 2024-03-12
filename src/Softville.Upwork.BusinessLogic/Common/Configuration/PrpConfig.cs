// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Softville.Upwork.BusinessLogic.Common.Configuration;

public class PrpConfig
{
    public required DbConfig Database { get; set; }
    public required UpworkConfig Upwork { get; set; }
}
