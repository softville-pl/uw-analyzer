// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Constructs;
using HashiCorp.Cdktf;
using HashiCorp.Cdktf.Providers.Azurerm.CosmosdbAccount;
using HashiCorp.Cdktf.Providers.Azurerm.DataAzurermClientConfig;
using HashiCorp.Cdktf.Providers.Azurerm.KeyVault;
using HashiCorp.Cdktf.Providers.Azurerm.KeyVaultSecret;
using HashiCorp.Cdktf.Providers.Azurerm.Provider;
using HashiCorp.Cdktf.Providers.Azurerm.ResourceGroup;

namespace Softville.Prospecting.Infra;

/// <summary>
/// </summary>
public class ProspectingAzureStack : TerraformStack
{
    /// <summary>
    /// </summary>
    /// <param name="scope"></param>
    /// <param name="id"></param>
    /// <param name="workload"></param>
    /// <param name="environment"></param>
    /// <param name="instance"></param>
    public ProspectingAzureStack(Construct scope, string id, string workload,
        string environment, string instance) : base(scope, id)
    {
        string namePostfix = $"{workload}-{environment}-plc-{instance}";

        new AzurermProvider(this, "azureFeature",
            new AzurermProviderConfig {Features = new AzurermProviderFeatures()});

        // Define the Resource Group
        Dictionary<string, string> tags = new() {{"env", environment}, {"app", "prospecting-app"}};

        ResourceGroup resourceGroup = new(this, "ResourceGroup",
            new ResourceGroupConfig {Name = $"rg-{namePostfix}", Location = "polandcentral", Tags = tags});

        DataAzurermClientConfig clientConfig = new(this, "ClientConfig");

        // Define KeyVault for the resource group
        KeyVault kv = new(this, "KeyVault",
            new KeyVaultConfig
            {
                Name = $"kv-{namePostfix}",
                ResourceGroupName = resourceGroup.Name,
                Location = resourceGroup.Location,
                SkuName = "standard",
                SoftDeleteRetentionDays = 7,
                TenantId = clientConfig.TenantId,
                Tags = tags,
                AccessPolicy = new IKeyVaultAccessPolicy[]
                {
                    new KeyVaultAccessPolicy
                    {
                        TenantId = clientConfig.TenantId,
                        ObjectId = clientConfig.ObjectId,
                        SecretPermissions = ["Get", "List", "Set", "Delete", "Recover", "Backup", "Restore"]
                    }
                }
            });

        // Define the CosmosDB Account
        CosmosdbAccount cosmosDb = new(this, "CosmosDbAccount",
            new CosmosdbAccountConfig
            {
                Name = $"cosmos-{namePostfix}",
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

        // Store the CosmosDB Account
        new KeyVaultSecret(this, "PrimaryKey",
            new KeyVaultSecretConfig
            {
                Name = $"{cosmosDb.Name}-primary-key",
                KeyVaultId = kv.Id,
                Value = cosmosDb.PrimaryKey,
                Tags = tags
            });

        // Store the CosmosDB Account
        new KeyVaultSecret(this, "PrimaryConnectionString",
            new KeyVaultSecretConfig
            {
                Name = $"{cosmosDb.Name}-primary-conn-string",
                KeyVaultId = kv.Id,
                Value = FnGenerated.Element(cosmosDb.ConnectionStrings, 0).ToString()!,
                Tags = tags
            });
    }
}
