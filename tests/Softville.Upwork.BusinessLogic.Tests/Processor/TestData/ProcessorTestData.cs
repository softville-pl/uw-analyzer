// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Softville.Upwork.BusinessLogic.Tests.Infrastructure;

namespace Softville.Upwork.BusinessLogic.Tests.Processor.TestData;

internal class ProcessorTestData
{
    internal static Stream GetCompleteUpworkOffer() =>
        typeof(ProcessorTestData).Assembly.GetResourceStream("upwork-fulldatamodel.json");

    internal static Stream UpworkSearchResult() =>
        typeof(ProcessorTestData).Assembly.GetResourceStream("upwork-searchresultmodel.json");
}
