// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using HashiCorp.Cdktf.Providers.Azurerm.KeyVault;
using HashiCorp.Cdktf.Providers.Azurerm.KeyVaultSecret;
using HashiCorp.Cdktf.Providers.Azurerm.RoleAssignment;
using HashiCorp.Cdktf.Providers.Azurerm.ServicePlan;
using HashiCorp.Cdktf.Providers.Azurerm.StorageAccount;
using HashiCorp.Cdktf.Providers.Azurerm.UserAssignedIdentity;
using HashiCorp.Cdktf.Providers.Azurerm.WindowsFunctionApp;

namespace Softville.Prospecting.Infra.Resources;

internal class ProcessorFunctionCreator
{
    internal static void CreateProcessorFunction(ResourceCreatorContext context, KeyVault kv,
        KeyVaultSecret storageAccessKeySecret,
        StorageAccount st)
    {
        UserAssignedIdentity managedIdentity = new(context.Scope, "AzureFunctionStorageIdentity",
            new UserAssignedIdentityConfig()
            {
                ResourceGroupName = context.ResourceGroup.Name,
                Location = context.ResourceGroup.Location,
                Name = $"id-{context.Infrastructure.GetResourceNamePostfix()}",
            });

        RoleAssignment kvRole = new(context.Scope, "KeyVaultIdentityScope",
            new RoleAssignmentConfig
            {
                Name = $"6b5f0c39-f45e-4db6-96ef-eac922266ee8",
                PrincipalId = managedIdentity.PrincipalId,
                Scope = kv.Id,
                RoleDefinitionName = "Key Vault Secrets User",
                DependsOn = [context.ResourceGroup, managedIdentity, kv]
            });

        var asp = new ServicePlan(context.Scope, "ServicePlan", new ServicePlanConfig
        {
            Name = $"asp-{context.Infrastructure.GetResourceNamePostfix()}",
            ResourceGroupName = context.ResourceGroup.Name,
            Location = context.ResourceGroup.Location,
            Tags = context.Tags,
            OsType = "Windows",
            SkuName = "Y1", //Free and B1 is the lowest paid sku
            ZoneBalancingEnabled = false
        });

        var secretUri = $"{kv.VaultUri}/secrets/${storageAccessKeySecret.Name}/${storageAccessKeySecret.Version}";

        _ = new WindowsFunctionApp(context.Scope, "AzureFunction",
            new WindowsFunctionAppConfig
            {
                Name = $"func-{context.Infrastructure.GetResourceNamePostfix()}",
                ResourceGroupName = context.ResourceGroup.Name,
                Location = context.ResourceGroup.Location,
                ServicePlanId = asp.Id,
                SiteConfig =
                    new WindowsFunctionAppSiteConfig
                    {
                        ApplicationStack = new WindowsFunctionAppSiteConfigApplicationStack()
                        {
                            DotnetVersion = "v8.0"
                        }
                    },
                AppSettings = new Dictionary<string, string>
                    {
                        ["WEBSITE_CONTENTAZUREFILECONNECTIONSTRING"] = $"@Microsoft.KeyVault(SecretUri=${secretUri})", ["WEBSITE_SKIP_CONTENTSHARE_VALIDATION"] = $"1"// https://github.com/Azure/azure-functions-host/issues/7094#issuecomment-1877521737
                    },
                StorageAccountName = st.Name,
                StorageAccountAccessKey = st.PrimaryAccessKey,
                Identity = new WindowsFunctionAppIdentity()
                {
                    Type = "SystemAssigned, UserAssigned", IdentityIds = [managedIdentity.Id]
                },
                KeyVaultReferenceIdentityId = managedIdentity.Id,
                Tags = context.Tags,
                DependsOn = [st, asp, managedIdentity, kvRole, storageAccessKeySecret]
            });
    }
}
