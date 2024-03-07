// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using HashiCorp.Cdktf;
using HashiCorp.Cdktf.Providers.Azurerm.CosmosdbAccount;
using HashiCorp.Cdktf.Providers.Azurerm.KeyVault;
using HashiCorp.Cdktf.Providers.Azurerm.KeyVaultSecret;

namespace Softville.Prospecting.Infra.Resources;

internal class CosmosDbCreator
{
    internal static CosmosdbAccount CreateCosmosDb(ResourceCreatorContext context, KeyVault kv, Dictionary<string, string> tags)
    {
        // Define the CosmosDB Account
        CosmosdbAccount cosmosDb = new(context.Scope, "CosmosDbAccount",
            new CosmosdbAccountConfig
            {
                Name = $"cosmos-{context.Infrastructure.GetNamePostfix()}",
                ResourceGroupName = context.ResourceGroup.Name,
                Location = context.ResourceGroup.Location,
                OfferType = "Standard",
                Kind = "MongoDB",
                ConsistencyPolicy = new CosmosdbAccountConsistencyPolicy {ConsistencyLevel = "Session"},
                GeoLocation =
                    new CosmosdbAccountGeoLocation[] {new() {Location = context.ResourceGroup.Location, FailoverPriority = 0}},
                EnableMultipleWriteLocations = false,
                EnableFreeTier = true,
                Capacity = new CosmosdbAccountCapacity {TotalThroughputLimit = 1000},
                Tags = context.ResourceGroup.Tags
            });

        // Store the CosmosDB Account
        _ = new KeyVaultSecret(context.Scope, "PrimaryKey",
            new KeyVaultSecretConfig
            {
                Name = $"{cosmosDb.Name}-primary-key", KeyVaultId = kv.Id, Value = cosmosDb.PrimaryKey, Tags = context.Tags
            });

        // Store the CosmosDB Account
        _ = new KeyVaultSecret(context.Scope, "PrimaryConnectionString",
            new KeyVaultSecretConfig
            {
                Name = $"{cosmosDb.Name}-primary-conn-string",
                KeyVaultId = kv.Id,
                Value = FnGenerated.Element(cosmosDb.ConnectionStrings, 0).ToString()!,
                Tags = tags
            });

        return cosmosDb;
    }
}
