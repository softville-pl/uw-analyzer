// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Net;
using Softville.Upwork.BusinessLogic.Processor.Parsers;
using Softville.Upwork.BusinessLogic.Processor.Storing;
using Softville.Upwork.BusinessLogic.Processor.UpworkApi;
using Softville.Upwork.Contracts;

namespace Softville.Upwork.BusinessLogic.Processor.Providers.ApplicantsStats;

internal class ApplicantsUpworkClient(IUpworkApiCaller apiCaller, IHttpResponseStoring storing) : IApplicantsClient
{
    public async Task<UpworkApplicantsStats> FetchApplicantsStatsAsync(UpworkId id, CancellationToken ct)
    {
        using HttpRequestMessage httpRequestMessage =
            new(HttpMethod.Get, $"job-details/jobdetails/api/job/{id.CipherText}/applicants");

        var response = await apiCaller.SendRequestAsync(httpRequestMessage, ct);

        await storing.PersistAsync(id, ApplicantsRequest.Instance, response, ct);

        if (response.IsSuccessStatusCode is false)
        {
            throw new WebException(
                $"Failed to query Upwork applicants for offer '{id}'. Response: {await response.Content.ReadAsStringAsync(ct)}");
        }

        return await UpworkParser.ParseAsync<UpworkApplicantsStats>(await response.Content.ReadAsStreamAsync(ct), ct);
    }
}
