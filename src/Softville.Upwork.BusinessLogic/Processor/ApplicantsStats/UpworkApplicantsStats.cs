// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Softville.Upwork.BusinessLogic.Processor.ApplicantsStats;

public class UpworkApplicantsStats
{
    public BidsStats? ApplicantsBidsStats { get; set; }
}

public class BidsStats
{
    public RateBid? AvgRateBid { get; set; }
    public RateBid? MinRateBid { get; set; }
    public RateBid? MaxRateBid { get; set; }
    public RateBid? AvgInterviewedRateBid { get; set; }
}

public class RateBid
{
    public required string CurrencyCode { get; set; }
    public decimal? Amount { get; set; }
}
