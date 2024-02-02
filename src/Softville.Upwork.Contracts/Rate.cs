// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Softville.Upwork.Contracts;

public class Rate
{
    public required decimal Minimum { get; set; }
    public required decimal Maximum { get; set; }
    public required string Currency { get; set; }
}
