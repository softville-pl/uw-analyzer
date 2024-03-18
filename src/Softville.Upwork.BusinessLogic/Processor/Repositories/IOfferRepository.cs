// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Softville.Upwork.Contracts;

namespace Softville.Upwork.BusinessLogic.Processor.Repositories;

public interface IOfferRepository
{
    Task<Offer> SaveAsync(Offer offer, CancellationToken ct);
    Task<Offer> GetAsync(string uid, CancellationToken ct);
    Task<List<Offer>> GetAllAsync(CancellationToken ct);
    Task<Offer> GetAsync(UpworkId id, CancellationToken ct);
}
