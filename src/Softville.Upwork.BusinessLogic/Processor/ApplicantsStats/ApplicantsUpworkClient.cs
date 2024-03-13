// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Softville.Upwork.BusinessLogic.Processor.Parsers;
using Softville.Upwork.BusinessLogic.Processor.UpworkApi;
using Softville.Upwork.Contracts;

namespace Softville.Upwork.BusinessLogic.Processor.ApplicantsStats;

internal class ApplicantsUpworkClient(IUpworkApiCaller apiCaller, IHttpResponsePersisting persisting) : IApplicantsClient
{
    public async Task<UpworkApplicantsStats> FetchApplicantsStatsAsync(UpworkId id, CancellationToken ct)
    {
        using HttpRequestMessage httpRequestMessage =
            new(HttpMethod.Get, $"job-details/jobdetails/api/job/{id.CipherText}/applicants");

        var response = await apiCaller.SendRequestAsync(httpRequestMessage, ct);

        await persisting.PersistAsync(id, ApplicantsRequest.Instance, response, ct);

        return await UpworkParser.ParseAsync<UpworkApplicantsStats>(await response.Content.ReadAsStreamAsync(ct), ct);
    }
}
