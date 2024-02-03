// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Softville.Upwork.Contracts;

public class Stats
{
    public int TotalAssignments { get; set; }
    public int? ActiveAssignmentsCount { get; set; }
    public int FeedbackCount { get; set; }
    public double Score { get; set; }
    public int TotalJobsWithHires { get; set; }
    public double HoursCount { get; set; }
    public TotalCharges? TotalCharges { get; set; }
}
