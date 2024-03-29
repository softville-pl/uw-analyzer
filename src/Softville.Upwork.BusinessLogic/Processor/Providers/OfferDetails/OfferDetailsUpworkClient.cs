﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Net;
using Softville.Upwork.BusinessLogic.Processor.Parsers;
using Softville.Upwork.BusinessLogic.Processor.Storing;
using Softville.Upwork.BusinessLogic.Processor.UpworkApi;
using Softville.Upwork.Contracts;

namespace Softville.Upwork.BusinessLogic.Processor.Providers.OfferDetails;

internal class OfferDetailsUpworkClient(IUpworkApiCaller apiCaller, IHttpResponseStoring storing) : IOfferDetailsUpworkClient
{
    public async Task<UpworkOffer> FetchDetailsAsync(UpworkId id, CancellationToken ct)
    {
        using HttpRequestMessage httpRequestMessage =
            new(HttpMethod.Get, $"job-details/jobdetails/api/job/{id.CipherText}/summary");

        var response = await apiCaller.SendRequestAsync(httpRequestMessage, ct);

        await storing.PersistAsync(id, DetailsRequest.Instance, response, ct);

        if (response.IsSuccessStatusCode is false)
        {
            throw new WebException(
                $"Failed to query Upwork details for '{id}'. Response: {await response.Content.ReadAsStringAsync(ct)}");
        }

        return await UpworkParser.ParseAsync<UpworkOffer>(await response.Content.ReadAsStreamAsync(ct), ct);
    }
}
