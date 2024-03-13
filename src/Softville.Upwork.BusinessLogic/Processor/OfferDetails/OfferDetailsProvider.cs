// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Softville.Upwork.Contracts;

namespace Softville.Upwork.BusinessLogic.Processor.OfferDetails;

internal class OfferDetailsProvider(IOfferDetailsUpworkClient detailsClient) : IOfferDetailsProvider
{
    public async Task<Offer> GetDetailsAsync(UpworkId id, CancellationToken ct)
    {
        var upworkOffer = await detailsClient.FetchDetailsAsync(id, ct);

        return upworkOffer.MapToOffer();
    }
}
