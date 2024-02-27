// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Softville.Upwork.Contracts;

namespace Softville.Upwork.BusinessLogic.Processor.ApplicantsStats;

internal static class ApplicantsStatsMapper
{
    internal static Contracts.ApplicantsStats MapToStats(this UpworkApplicantsStats stats)
    {
        return new Contracts.ApplicantsStats
        {
            MinRateBid = stats.ApplicantsBidsStats?.MinRateBid?.MapToBid() ?? BidsDetails.Empty,
            AvgRateBid = stats.ApplicantsBidsStats?.AvgRateBid?.MapToBid() ?? BidsDetails.Empty,
            MaxRateBid = stats.ApplicantsBidsStats?.MaxRateBid?.MapToBid() ?? BidsDetails.Empty,
            AvgInterviewedRateBid =
                stats.ApplicantsBidsStats?.AvgInterviewedRateBid?.MapToBid() ?? BidsDetails.Empty
        };
    }

    private static BidsDetails MapToBid(this RateBid bid)
    {
        return new BidsDetails {CurrencyCode = bid.CurrencyCode, Amount = bid.Amount ?? -1};
    }
}
