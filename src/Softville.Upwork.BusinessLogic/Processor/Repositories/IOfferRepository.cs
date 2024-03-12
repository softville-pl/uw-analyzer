// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Softville.Upwork.BusinessLogic.Common.Configuration;

namespace Softville.Upwork.BusinessLogic.Processor.Repositories;

public interface IOfferRepository
{
    Task ConnectAsync(CancellationToken ct);
}

internal class OfferRepository(IOptions<DbConfig> dbConfig) : IOfferRepository
{
    public async Task ConnectAsync(CancellationToken ct)
    {
        System.Console.WriteLine(dbConfig.Value.ConnectionString);

        if (dbConfig.Value.ConnectionString is null)
            throw new InvalidOperationException("DbConfig is null");

        var client = new MongoClient(dbConfig.Value.ConnectionString);

        var database = client.GetDatabase("cosmicworks");
        var collection = database.GetCollection<dynamic>("products");

        var item = new {name = "Kiama classic surfboard"};

        await collection.InsertOneAsync(item, null, ct);

        // var readItem = await collection.FindAsync(f => f.name == "Kiama classic surfboard");
        // readItem.
            var dbs = await client.ListDatabasesAsync(ct);
        var count = dbs.ToList(ct).Count;
        await Task.CompletedTask;
        var cluster = client.Cluster;
        var settings = client.Settings;
    }
}
