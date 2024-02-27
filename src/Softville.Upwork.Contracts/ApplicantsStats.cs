// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Softville.Upwork.Contracts;

public class ApplicantsStats
{
    public BidsDetails MinRateBid { get; set; } = BidsDetails.Empty;
    public BidsDetails AvgRateBid { get; set; } = BidsDetails.Empty;
    public BidsDetails MaxRateBid { get; set; } = BidsDetails.Empty;
    public BidsDetails AvgInterviewedRateBid { get; set; } = BidsDetails.Empty;
}
