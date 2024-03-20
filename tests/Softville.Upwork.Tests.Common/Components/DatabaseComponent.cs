// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using MongoDB.Driver;
using Testcontainers.MongoDb;

namespace Softville.Upwork.Tests.Common.Components;

public class DatabaseComponent
{
    private readonly MongoDbContainer _dbContainer = new MongoDbBuilder()
        .WithImage("mongodb/mongodb-community-server:latest")
        .Build();

    public string ConnectionString => _dbContainer.GetConnectionString();

    public async Task StartAsync(CancellationToken ct) => await _dbContainer.StartAsync(ct);

    public async Task CleanupDatabaseAsync()
    {
        var mongoClient = new MongoClient(ConnectionString);
        await mongoClient.DropDatabaseAsync("Prospecting");
    }
}
