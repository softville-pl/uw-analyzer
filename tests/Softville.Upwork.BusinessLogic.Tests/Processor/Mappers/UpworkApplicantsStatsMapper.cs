// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Softville.Upwork.BusinessLogic.Processor.ApplicantsStats;
using Softville.Upwork.BusinessLogic.Processor.Parsers;
using Softville.Upwork.BusinessLogic.Tests.Processor.TestData;

namespace Softville.Upwork.BusinessLogic.Tests.Processor.Mappers;

public class UpworkApplicantsStatsMapper
{
    [Fact]
    public async Task GivenUpworkApplicantsStats_WhenMapToBids_ThenCorrectlyMapped()
    {
        await using Stream stream = ProcessorTestData.UpworkApplicants();

        CancellationToken ct = CancellationToken.None;

        var bidsStats = await UpworkParser.ParseAsync<UpworkApplicantsStats>(stream, ct);

        await Verify(bidsStats.MapToStats());
    }
}
