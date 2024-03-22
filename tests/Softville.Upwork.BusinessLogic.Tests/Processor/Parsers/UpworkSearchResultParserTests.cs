// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Softville.Upwork.BusinessLogic.Processor.Parsers;
using Softville.Upwork.Tests.Common.Data;

namespace Softville.Upwork.BusinessLogic.Tests.Processor.Parsers;

public class UpworkSearchResultParserTests
{
    [Fact(DisplayName = "Search result parses correctly")]
    public async Task GivenSuccessfulSearchResult_WhenParse_ThenOk()
    {
        await using Stream stream = TestData.UpworkSearchResult();
        UpworkSearchResult actual =
            await UpworkSearchResultParser.ParseSearchResults(stream, CancellationToken.None);
        await Verify(actual);
    }
}
