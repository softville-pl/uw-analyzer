// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Constructs;
using HashiCorp.Cdktf;
using HashiCorp.Cdktf.Providers.Azurerm.DataAzurermClientConfig;
using HashiCorp.Cdktf.Providers.Azurerm.KeyVault;
using HashiCorp.Cdktf.Providers.Azurerm.Provider;
using HashiCorp.Cdktf.Providers.Azurerm.ResourceGroup;
using Softville.Prospecting.Infra.Resources;

namespace Softville.Prospecting.Infra;

/// <summary>
/// </summary>
public class ProspectingAzureStack : TerraformStack
{
    /// <summary>
    /// </summary>
    /// <param name="scope"></param>
    /// <param name="id"></param>
    /// <param name="infrastructure"></param>
    public ProspectingAzureStack(Construct scope, string id, InfrastructureInstance infrastructure) : base(scope, id)
    {
        string namePostfix = infrastructure.GetNamePostfix();

        new AzurermProvider(this, "azureFeature",
            new AzurermProviderConfig {Features = new AzurermProviderFeatures()});

        Dictionary<string, string> tags = new() {{"env", infrastructure.Environment}, {"app", "prospecting-app"}};

        // Define the Resource Group
        ResourceGroup resourceGroup = new(this, "ResourceGroup",
            new ResourceGroupConfig {Name = $"rg-{namePostfix}", Location = "polandcentral", Tags = tags});

        DataAzurermClientConfig clientConfig = new(this, "ClientConfig");
        var adminUserObjectId = "035e7cd0-50b7-49bb-bff5-4134329917a4";

        var context = new ResourceCreatorContext(this, resourceGroup, clientConfig, adminUserObjectId, infrastructure, tags);

        // Define KeyVault for the resource group
        KeyVault kv = KeyVaultCreator.CreateKeyVault(context);

        _ = CosmosDbCreator.CreateCosmosDb(context, kv, tags);

        // StorageAccount st = new(this, "StorageAccount",
        //     new StorageAccountConfig
        //     {
        //         Name = $"st-{namePostfix}",
        //         ResourceGroupName = resourceGroup.Name,
        //         Location = resourceGroup.Location,
        //         AccountKind = "StorageV2",
        //         AccountTier = "Standard",
        //         AccountReplicationType = "LRS",
        //         MinTlsVersion = "TLS1_2",
        //         EnableHttpsTrafficOnly = true,
        //         PublicNetworkAccessEnabled = false,
        //         SharedAccessKeyEnabled = true,
        //         NetworkRules =
        //             new StorageAccountNetworkRules {DefaultAction = "Allow", Bypass = ["Logging", "AzureServices"]},
        //         BlobProperties = new StorageAccountBlobProperties
        //         {
        //             DeleteRetentionPolicy = new StorageAccountBlobPropertiesDeleteRetentionPolicy {Days = 7,},
        //             ContainerDeleteRetentionPolicy =
        //                 new StorageAccountBlobPropertiesContainerDeleteRetentionPolicy {Days = 7},
        //         },
        //         AccessTier = "Hot"
        //     });
        //
        // _ = new KeyVaultSecret(this, "StorageAccountPrimaryAccessKey",
        //     new KeyVaultSecretConfig
        //     {
        //         Name = $"{st.Name}-primary-access-key", KeyVaultId = kv.Id, Value = st.PrimaryAccessKey, Tags = tags
        //     });
        //
        // var asp = new ServicePlan(this, "ServicePlan", new ServicePlanConfig
        // {
        //     Name = $"asp-{namePostfix}",
        //     ResourceGroupName = resourceGroup.Name,
        //     Location = resourceGroup.Location,
        //     Tags = tags,
        //     OsType = "Linux",
        //     SkuName = "Y1", //Free and B1 is the lowest paid sku
        //     ZoneBalancingEnabled = false
        // });
        //
        // var fnc = new FunctionApp(this, "AzureFunction",
        //     new FunctionAppConfig
        //     {
        //         Name = $"func-{namePostfix}",
        //         ResourceGroupName = resourceGroup.Location,
        //         Location = resourceGroup.Location,
        //         AppServicePlanId = asp.Id,
        //         OsType = "linux",
        //         SiteConfig = new FunctionAppSiteConfig {DotnetFrameworkVersion = "v8.0"},
        //         AppSettings = new Dictionary<string, string>() { },
        //         StorageAccountName = st.Name,
        //         StorageAccountAccessKey = st.PrimaryAccessKey,
        //         Tags = tags,
        //         KeyVaultReferenceIdentityId =
        //     });

        // var fnc2 = new LinuxFunctionApp(this, "AzureFunction", new LinuxFunctionAppConfig()
        // {
        //     Name = $"func-{namePostfix}",
        //     ResourceGroupName = resourceGroup.Location,
        //     Location = resourceGroup.Location,
        //     ServicePlanId = asp.Id,
        //     AppSettings = new Dictionary<string, string>
        //     {
        //         { "FUNCTIONS_WORKER_RUNTIME", "dotnet-isolated" },
        //         { "FUNCTIONS_EXTENSION_VERSION", "~4" }
        //         // Add other settings as needed
        //     },
        //     StorageAccountName = st.Name,
        //     StorageAccountAccessKey = st.PrimaryAccessKey
        // });
    }
}
