// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Softville.Upwork.Contracts;

namespace Softville.Upwork.BusinessLogic.Processor.Providers.ApplicantsStats;

internal class ApplicantsStatsProvider(IApplicantsClient applicantsClient) : IApplicantsStatsProvider
{
    public async Task<Contracts.ApplicantsStats> GetBidsStatsAsync(UpworkId id, CancellationToken ct)
    {
        var applicantsStats = await applicantsClient.FetchApplicantsStatsAsync(id, ct);

        return applicantsStats.MapToStats();
    }
}
