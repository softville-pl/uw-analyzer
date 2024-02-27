// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Softville.Upwork.Contracts;

namespace Softville.Upwork.BusinessLogic.Processor.ApplicantsStats;

internal interface IApplicantsClient
{
    Task<UpworkApplicantsStats> GetApplicantsStatsAsync(UpworkId id, CancellationToken ct);
}
