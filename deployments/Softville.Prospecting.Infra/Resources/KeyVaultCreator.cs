// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using HashiCorp.Cdktf;
using HashiCorp.Cdktf.Providers.Azurerm.KeyVault;
using HashiCorp.Cdktf.Providers.Azurerm.KeyVaultAccessPolicy;

namespace Softville.Prospecting.Infra.Resources;

/// <summary>
///
/// </summary>
internal class KeyVaultCreator
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    internal static KeyVault CreateKeyVault(ResourceCreatorContext context)
    {
        KeyVault kv = new(context.Scope, "KeyVault",
            new KeyVaultConfig
            {
                Name = $"kv-{context.Infrastructure.GetNamePostfix()}",
                ResourceGroupName = context.ResourceGroup.Name,
                Location = context.ResourceGroup.Location,
                SkuName = "standard",
                SoftDeleteRetentionDays = 7,
                TenantId = context.ClientConfig.TenantId,
                Tags = context.ResourceGroup.Tags,
                Lifecycle = new TerraformResourceLifecycle()
                {
                    CreateBeforeDestroy = true
                }
            });

        return kv;
    }

    internal static KeyVaultAccessPolicyA CreateFullSecretAccessPolicy(ResourceCreatorContext context,
        KeyVault keyVault,
        string name, string objectId)
    {
        return new KeyVaultAccessPolicyA(context.Scope, name,
            new KeyVaultAccessPolicyAConfig
            {
                TenantId = context.ClientConfig.TenantId,
                ObjectId = objectId,
                KeyVaultId = keyVault.Id,
                SecretPermissions = ["Get", "List", "Set", "Delete", "Recover", "Backup", "Restore"],
                DependsOn = [keyVault]
            });
    }
}
