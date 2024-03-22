// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Softville.Upwork.Contracts;

namespace Softville.Upwork.Tests.Common.Data;

public class TestData
{
    private static readonly CancellationToken _ct = CancellationToken.None;
    private static readonly UpworkId _offer1Id = new("1745755393334956032", "~019bb7b2a2c6f33572");

    public static ResourceOffer Offer1DetailsV1 { get; } = new (_offer1Id, "upwork-fulldatamodel.json");
    public static ResourceOffer Offer1ApplicantsV1 { get; } = new (_offer1Id, "upwork-applicantsmodel.json");

    public static Task<string> GetCompleteUpworkOfferV2Text() =>
        typeof(TestData).Assembly.GetResourceTextAsync("upwork-fulldatamodel-v2.json", _ct);

    public static Stream UpworkSearchResult() =>
        typeof(TestData).Assembly.GetResourceStream("upwork-searchresultmodel.json");

    public static Task<string> UpworkSearchResultText() =>
        typeof(TestData).Assembly.GetResourceTextAsync("upwork-searchresultmodel.json",_ct);

    public static Stream UpworkApplicants() =>
        typeof(TestData).Assembly.GetResourceStream("upwork-applicantsmodel.json");

    public static Task<string> UpworkApplicantsV2Text() =>
        typeof(TestData).Assembly.GetResourceTextAsync("upwork-applicantsmodel-v2.json", _ct);
}
