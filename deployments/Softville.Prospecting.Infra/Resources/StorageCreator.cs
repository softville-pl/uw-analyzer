// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using HashiCorp.Cdktf.Providers.Azurerm.StorageAccount;

namespace Softville.Prospecting.Infra.Resources;

internal class StorageCreator
{
    internal static StorageAccount CreateStorageAccount(ResourceCreatorContext context)
    {
        StorageAccount st = new(context.Scope, "StorageAccount",
            new StorageAccountConfig
            {
                Name = $"st{context.Infrastructure.GetStorageAccountNameNamePostfix()}",
                ResourceGroupName = context.ResourceGroup.Name,
                Location = context.ResourceGroup.Location,
                AccountKind = "StorageV2",
                AccountTier = "Standard",
                AccountReplicationType = "LRS",
                MinTlsVersion = "TLS1_2",
                EnableHttpsTrafficOnly = true,
                PublicNetworkAccessEnabled = false,
                SharedAccessKeyEnabled = true,
                NetworkRules =
                    new StorageAccountNetworkRules {DefaultAction = "Allow", Bypass = ["Logging", "AzureServices"]},
                BlobProperties = new StorageAccountBlobProperties
                {
                    DeleteRetentionPolicy = new StorageAccountBlobPropertiesDeleteRetentionPolicy {Days = 7,},
                    ContainerDeleteRetentionPolicy =
                        new StorageAccountBlobPropertiesContainerDeleteRetentionPolicy {Days = 7},
                },
                AccessTier = "Hot"
            });
        return st;
    }
}
