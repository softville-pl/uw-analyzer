// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Testcontainers.MongoDb;

namespace Softville.Upwork.BusinessLogic.IntTests.Infrastructure;

public class TestDatabase
{
    private readonly MongoDbContainer _dbContainer = new MongoDbBuilder()
        .WithImage("mongodb/mongodb-community-server:latest")
        .Build();

    // private readonly CosmosDbContainer _dbContainer = new CosmosDbBuilder()
    //     .WithImage("mcr.microsoft.com/cosmosdb/linux/azure-cosmos-emulator:latest")
    //     .Build();

    public string ConnectionString => _dbContainer.GetConnectionString();

    public async Task StartAsync(CancellationToken ct) => await _dbContainer.StartAsync(ct);
}
