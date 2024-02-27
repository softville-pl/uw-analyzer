﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Softville.Upwork.BusinessLogic.Processor;
using Softville.Upwork.BusinessLogic.Processor.Parsers;
using Softville.Upwork.BusinessLogic.Tests.Processor.TestData;

namespace Softville.Upwork.BusinessLogic.Tests.Processor.Mappers;

public class UpworkOfferMapper
{
    [Fact(DisplayName = "UpworkOffer is mapped to Offer")]
    public async Task GivenUpworkOffer_WhenMapToOffer_ThenMapped()
    {
        await using Stream stream = ProcessorTestData.GetCompleteUpworkOffer();

        UpworkOffer expected =
            await UpworkParser.ParseAsync<UpworkOffer>(stream, CancellationToken.None);

        await Verify(expected.MapToOffer());
    }
}
