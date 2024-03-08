// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using HashiCorp.Cdktf.Providers.Azurerm.ResourceGroup;
using HashiCorp.Cdktf.Providers.Azurerm.RoleAssignment;

namespace Softville.Prospecting.Infra.Resources;

internal class ResourceGroupCreator
{
    internal static ResourceGroup CreateResourceGroup(ResourceCreatorContext context)
    {
        ResourceGroup resourceGroup = new(context.Scope, "ResourceGroup",
            new ResourceGroupConfig
            {
                Name = $"rg-{context.Infrastructure.GetResourceNamePostfix()}",
                Location = "polandcentral",
                Tags = context.Tags
            });

         _ = new RoleAssignment(context.Scope, "ServicePrincipalResourceGroupOwner",
            new RoleAssignmentConfig
            {
                Name = "3ea4f156-9597-48ca-a559-ccc3d4292956",
                PrincipalId = context.ClientConfig.ObjectId,
                Scope = resourceGroup.Id,
                RoleDefinitionName = "Owner",
                DependsOn = [resourceGroup]
            });

        return resourceGroup;
    }
}
