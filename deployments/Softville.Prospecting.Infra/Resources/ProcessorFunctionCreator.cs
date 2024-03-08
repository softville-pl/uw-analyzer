// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using HashiCorp.Cdktf.Providers.Azurerm.FunctionApp;
using HashiCorp.Cdktf.Providers.Azurerm.KeyVault;
using HashiCorp.Cdktf.Providers.Azurerm.KeyVaultSecret;
using HashiCorp.Cdktf.Providers.Azurerm.RoleAssignment;
using HashiCorp.Cdktf.Providers.Azurerm.ServicePlan;
using HashiCorp.Cdktf.Providers.Azurerm.StorageAccount;
using HashiCorp.Cdktf.Providers.Azurerm.UserAssignedIdentity;

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
            new RoleAssignmentConfig()
            {
                Name = $"kv-role{context.Infrastructure.GetResourceNamePostfix()}",
                PrincipalId = managedIdentity.PrincipalId,
                Scope = kv.Id,
                RoleDefinitionName = "Key Vault Secrets User",
                DependsOn = [managedIdentity, kv]
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

        _ = new FunctionApp(context.Scope, "AzureFunction",
            new FunctionAppConfig
            {
                Name = $"func-{context.Infrastructure.GetResourceNamePostfix()}",
                ResourceGroupName = context.ResourceGroup.Location,
                Location = context.ResourceGroup.Location,
                AppServicePlanId = asp.Id,
                SiteConfig = new FunctionAppSiteConfig {DotnetFrameworkVersion = "v8.0"},
                AppSettings =
                    new Dictionary<string, string>
                    {
                        ["WEBSITE_CONTENTAZUREFILECONNECTIONSTRING"] =
                            $"@Microsoft.KeyVault(SecretUri=${storageAccessKeySecret.Id})"
                    },
                StorageAccountName = st.Name,
                StorageAccountAccessKey = st.PrimaryAccessKey,
                Identity = new FunctionAppIdentity
                {
                    Type = "SystemAssigned,UserAssigned", IdentityIds = [managedIdentity.Id]
                },
                KeyVaultReferenceIdentityId = managedIdentity.Id,
                Tags = context.Tags,
                DependsOn = [st, asp, managedIdentity, kvRole, storageAccessKeySecret]
            });
    }

}
