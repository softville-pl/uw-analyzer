// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using HashiCorp.Cdktf.Providers.Azurerm.ResourceGroup;

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

        // This role assigment has to be done manually (SP cannot assign this role for itself)
        // _ = new RoleAssignment(context.Scope, "ServicePrincipalResourceGroupRBACAdmin",
        //    new RoleAssignmentConfig
        //    {
        //        Name = "3ea4f156-9597-48ca-a559-ccc3d4292956",
        //        PrincipalId = context.ClientConfig.ObjectId,
        //        Scope = resourceGroup.Id,
        //        RoleDefinitionName = "Role Based Access Control Administrator",
        //        DependsOn = [resourceGroup]
        //    });

        return resourceGroup;
    }
}
