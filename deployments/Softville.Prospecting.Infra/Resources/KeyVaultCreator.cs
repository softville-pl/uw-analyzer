// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using HashiCorp.Cdktf.Providers.Azurerm.KeyVault;

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
                Name = $"kv-{context.Infrastructure.GetResourceNamePostfix()}",
                ResourceGroupName = context.ResourceGroup.Name,
                Location = context.ResourceGroup.Location,
                SkuName = "standard",
                SoftDeleteRetentionDays = 7,
                TenantId = context.ClientConfig.TenantId,
                Tags = context.ResourceGroup.Tags,
                AccessPolicy = new IKeyVaultAccessPolicy[]
                {
                    new KeyVaultAccessPolicy
                    {
                        TenantId = context.ClientConfig.TenantId,
                        ObjectId = context.ClientConfig.ObjectId,
                        SecretPermissions = ["Get", "List", "Set", "Delete", "Recover", "Backup", "Restore"]
                    },
                    new KeyVaultAccessPolicy
                    {
                        TenantId = context.ClientConfig.TenantId,
                        ObjectId = context.AdminUserObjectId,
                        SecretPermissions = ["Get", "List", "Set", "Delete", "Recover", "Backup", "Restore"]
                    }
                }
            });
        return kv;
    }
}
