// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Constructs;
using HashiCorp.Cdktf;
using HashiCorp.Cdktf.Providers.Azurerm.DataAzurermClientConfig;
using HashiCorp.Cdktf.Providers.Azurerm.KeyVault;
using HashiCorp.Cdktf.Providers.Azurerm.KeyVaultSecret;
using HashiCorp.Cdktf.Providers.Azurerm.Provider;
using HashiCorp.Cdktf.Providers.Azurerm.ResourceGroup;
using HashiCorp.Cdktf.Providers.Azurerm.StorageAccount;
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
        string namePostfix = infrastructure.GetResourceNamePostfix();

        new AzurermProvider(this, "azureFeature",
            new AzurermProviderConfig {Features = new AzurermProviderFeatures()});

        Dictionary<string, string> tags = new() {{"env", infrastructure.Environment}, {"app", "prospecting-app"}};

        ResourceGroup resourceGroup = new(this, "ResourceGroup",
            new ResourceGroupConfig {Name = $"rg-{namePostfix}", Location = "polandcentral", Tags = tags});

        DataAzurermClientConfig clientConfig = new(this, "ClientConfig");
        var adminUserObjectId = "035e7cd0-50b7-49bb-bff5-4134329917a4";

        var context =
            new ResourceCreatorContext(this, resourceGroup, clientConfig, adminUserObjectId, infrastructure, tags);

        KeyVault kv = KeyVaultCreator.CreateKeyVault(context);

        _ = CosmosDbCreator.CreateCosmosDb(context, kv);

        StorageAccount st = StorageCreator.CreateStorageAccount(context, resourceGroup);

        var storageAccessKeySecret = new KeyVaultSecret(this, "StorageAccountPrimaryAccessKey",
            new KeyVaultSecretConfig
            {
                Name = $"{st.Name}-primary-access-key", KeyVaultId = kv.Id, Value = st.PrimaryAccessKey, Tags = tags,
            });

        ProcessorFunctionCreator.CreateProcessorFunction(context, kv, storageAccessKeySecret, st);
    }
}
