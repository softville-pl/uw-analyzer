// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Softville.Upwork.Tests.Common.Data;

public class TestData
{
    public static Stream GetCompleteUpworkOffer() =>
        typeof(TestData).Assembly.GetResourceStream("upwork-fulldatamodel.json");

    public static Stream UpworkSearchResult() =>
        typeof(TestData).Assembly.GetResourceStream("upwork-searchresultmodel.json");

    public static Stream UpworkApplicants() =>
        typeof(TestData).Assembly.GetResourceStream("upwork-applicantsmodel.json");
}
