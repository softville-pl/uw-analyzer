// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Softville.Upwork.Tests.Common.Data;

public class TestData
{
    private static readonly CancellationToken _ct = CancellationToken.None;

    public static Stream GetCompleteUpworkOffer() =>
        typeof(TestData).Assembly.GetResourceStream("upwork-fulldatamodel.json");

    public static Task<string> GetCompleteUpworkOfferText() =>
        typeof(TestData).Assembly.GetResourceTextAsync("upwork-fulldatamodel.json", _ct);

    public static Task<string> GetCompleteUpworkOfferV2Text() =>
        typeof(TestData).Assembly.GetResourceTextAsync("upwork-fulldatamodel-v2.json", _ct);

    public static Stream UpworkSearchResult() =>
        typeof(TestData).Assembly.GetResourceStream("upwork-searchresultmodel.json");

    public static Task<string> UpworkSearchResultText() =>
        typeof(TestData).Assembly.GetResourceTextAsync("upwork-searchresultmodel.json",_ct);

    public static Stream UpworkApplicants() =>
        typeof(TestData).Assembly.GetResourceStream("upwork-applicantsmodel.json");

    public static Task<string> UpworkApplicantsText() =>
        typeof(TestData).Assembly.GetResourceTextAsync("upwork-applicantsmodel.json", _ct);

    public static Task<string> UpworkApplicantsV2Text() =>
        typeof(TestData).Assembly.GetResourceTextAsync("upwork-applicantsmodel-v2.json", _ct);
}
