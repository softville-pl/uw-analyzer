// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Softville.Upwork.BusinessLogic.Processor.Repositories;
using Softville.Upwork.Contracts;

namespace Softville.Upwork.BusinessLogic.Queries.Offers;

public class GetOffersQuery(IOfferRepository offerRepository) : IGetOffersQuery
{
    public Task<List<Offer>> GetOffersAsync(CancellationToken ct)
    {
        return offerRepository.GetAllAsync(ct);
    }
}
