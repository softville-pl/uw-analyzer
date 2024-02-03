// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Softville.Upwork.Contracts;

public class Cooperation
{
    public required string DurationLabel { get; set; }
    public required string Workload { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public int? NumberOfPositionsToHire { get; set; }
}
