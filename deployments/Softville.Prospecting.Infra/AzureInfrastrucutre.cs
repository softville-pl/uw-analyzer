// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Constructs;
using HashiCorp.Cdktf;
using HashiCorp.Cdktf.Providers.Azurerm.CosmosdbAccount;
using HashiCorp.Cdktf.Providers.Azurerm.Provider;
using HashiCorp.Cdktf.Providers.Azurerm.ResourceGroup;

namespace Softville.Prospecting.Infra;

public class AzureInfrastructure : TerraformStack
{
    public AzureInfrastructure(Construct scope, string id, string resourceGroupName, string cosmosDbName,
        string environment) : base(scope, id)
    {
        new AzurermProvider(this, "azureFeature", new AzurermProviderConfig {Features = new AzurermProviderFeatures()});

        // Define the Resource Group
        Dictionary<string, string> tags = new() {{"env", environment}, {"app", "prospecting-app"}};

        ResourceGroup resourceGroup = new(this, "ResourceGroup",
            new ResourceGroupConfig {Name = resourceGroupName, Location = "polandcentral", Tags = tags});

        // Define the CosmosDB Account
        new CosmosdbAccount(this, "CosmosDbAccount",
            new CosmosdbAccountConfig
            {
                Name = cosmosDbName,
                ResourceGroupName = resourceGroup.Name,
                Location = resourceGroup.Location,
                OfferType = "Standard",
                Kind = "GlobalDocumentDB",
                ConsistencyPolicy = new CosmosdbAccountConsistencyPolicy {ConsistencyLevel = "Session"},
                GeoLocation =
                    new CosmosdbAccountGeoLocation[] {new() {Location = resourceGroup.Location, FailoverPriority = 0}},
                EnableMultipleWriteLocations = false,
                EnableFreeTier = true,
                Capacity = new CosmosdbAccountCapacity {TotalThroughputLimit = 1000},
                Tags = tags
            });
    }
}
