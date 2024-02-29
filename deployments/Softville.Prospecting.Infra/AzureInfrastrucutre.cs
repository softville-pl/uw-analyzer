// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Constructs;
using HashiCorp.Cdktf;
using HashiCorp.Cdktf.Providers.Azurerm.CosmosdbAccount;
using HashiCorp.Cdktf.Providers.Azurerm.DataAzurermClientConfig;
using HashiCorp.Cdktf.Providers.Azurerm.KeyVault;
using HashiCorp.Cdktf.Providers.Azurerm.KeyVaultSecret;
using HashiCorp.Cdktf.Providers.Azurerm.LinuxFunctionApp;
using HashiCorp.Cdktf.Providers.Azurerm.Provider;
using HashiCorp.Cdktf.Providers.Azurerm.ResourceGroup;
using HashiCorp.Cdktf.Providers.Azurerm.ServicePlan;
using HashiCorp.Cdktf.Providers.Azurerm.StorageAccount;

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
        var adminUserObjectId = "035e7cd0-50b7-49bb-bff5-4134329917a4";

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
                    },
                    new KeyVaultAccessPolicy
                    {
                        TenantId = clientConfig.TenantId,
                        ObjectId = adminUserObjectId,
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
                Kind = "MongoDB",
                ConsistencyPolicy = new CosmosdbAccountConsistencyPolicy {ConsistencyLevel = "Session"},
                GeoLocation =
                    new CosmosdbAccountGeoLocation[] {new() {Location = resourceGroup.Location, FailoverPriority = 0}},
                EnableMultipleWriteLocations = false,
                EnableFreeTier = true,
                Capacity = new CosmosdbAccountCapacity {TotalThroughputLimit = 1000},
                Tags = tags
            });

        // Store the CosmosDB Account
        _ = new KeyVaultSecret(this, "PrimaryKey",
            new KeyVaultSecretConfig
            {
                Name = $"{cosmosDb.Name}-primary-key", KeyVaultId = kv.Id, Value = cosmosDb.PrimaryKey, Tags = tags
            });

        // Store the CosmosDB Account
        _ = new KeyVaultSecret(this, "PrimaryConnectionString",
            new KeyVaultSecretConfig
            {
                Name = $"{cosmosDb.Name}-primary-conn-string",
                KeyVaultId = kv.Id,
                Value = FnGenerated.Element(cosmosDb.ConnectionStrings, 0).ToString()!,
                Tags = tags
            });

        StorageAccount st = new(this, "StorageAccount",
            new StorageAccountConfig
            {
                Name = $"st-{namePostfix}",
                ResourceGroupName = resourceGroup.Name,
                Location = resourceGroup.Location,
                AccountKind = "StorageV2",
                AccountTier = "Standard",
                AccountReplicationType = "LRS",
                MinTlsVersion = "TLS1_2",
                EnableHttpsTrafficOnly = true,
                PublicNetworkAccessEnabled = false,
                SharedAccessKeyEnabled = true,
                NetworkRules =
                new StorageAccountNetworkRules
                {
                    DefaultAction = "Allow",
                    Bypass = ["Logging", "AzureServices"]
                },
                BlobProperties = new StorageAccountBlobProperties
                {
                    DeleteRetentionPolicy = new StorageAccountBlobPropertiesDeleteRetentionPolicy
                    {
                        Days = 7,
                    },
                    ContainerDeleteRetentionPolicy = new StorageAccountBlobPropertiesContainerDeleteRetentionPolicy
                    {
                        Days = 7
                    },
                },
                AccessTier = "Hot"
            });

        _ = new KeyVaultSecret(this, "StorageAccountPrimaryAccessKey",
            new KeyVaultSecretConfig
            {
                Name = $"{st.Name}-primary-access-key",
                KeyVaultId = kv.Id,
                Value = st.PrimaryAccessKey,
                Tags = tags
            });

        var asp = new ServicePlan(this, "ServicePlan", new ServicePlanConfig
        {
            Name = $"asp-{namePostfix}",
            ResourceGroupName = resourceGroup.Name,
            Location = resourceGroup.Location,
            Tags = tags,
            OsType = "Linux",
            SkuName = "Y1", //Free and B1 is the lowest paid sku
            ZoneBalancingEnabled = false
        });

        // var fnc = new FunctionApp(this, "AzureFunction", new FunctionAppConfig()
        // {
        //     Name = $"func-{namePostfix}",
        //     ResourceGroupName = resourceGroup.Location,
        //     Location = resourceGroup.Location,
        //     OsType = "linux",
        //     SiteConfig = new FunctionAppSiteConfig {DotnetFrameworkVersion = "v8.0"},
        //     AppSettings = new Dictionary<string, string>()
        //     Tags = tags
        // });

        var fnc2 = new LinuxFunctionApp(this, "AzureFunction", new LinuxFunctionAppConfig()
        {
            Name = $"func-{namePostfix}",
            ResourceGroupName = resourceGroup.Location,
            Location = resourceGroup.Location,
            ServicePlanId = asp.Id,
            AppSettings = new Dictionary<string, string>
            {
                { "FUNCTIONS_WORKER_RUNTIME", "dotnet-isolated" },
                { "FUNCTIONS_EXTENSION_VERSION", "~4" }
                // Add other settings as needed
            },
            StorageAccountName = st.Name,
            StorageAccountAccessKey = st.PrimaryAccessKey
        });
    }
}
