// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Softville.Upwork.Contracts;

namespace Softville.Upwork.BusinessLogic.Processor.Repositories;

internal class OfferRepository(IMongoDatabase database)
    : IOfferRepository
{
    private const string OffersCol = "Offers";

    public async Task<Offer> SaveAsync(Offer offer, CancellationToken ct)
    {
        var collection = database.GetCollection<Offer>(OffersCol);

        await collection.InsertOneAsync(offer, new InsertOneOptions(), ct);

        return offer;
    }

    public Task<List<Offer>> GetAllAsync(CancellationToken ct)
    {
        return database
            .GetCollection<Offer>(OffersCol)
            .AsQueryable()
            .ToListAsync(ct);
    }

    public Task<Offer> GetAsync(string uid, CancellationToken ct) =>
        database
            .GetCollection<Offer>(OffersCol)
            .AsQueryable()
            .FirstOrDefaultAsync(o => o.Uid == uid, ct);

    public Task<Offer> GetAsync(UpworkId id, CancellationToken ct) => GetAsync(id.Uid, ct);
}
