// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Softville.Upwork.Contracts;

public class Qualification
{
    public bool Qualified { get; set; }
    public string? ClientPreferred { get; set; }
    public string? FreelancerValue { get; set; }
    public string? FreelancerValueLabel { get; set; }
    public string? ClientPreferredLabel { get; set; }
}
