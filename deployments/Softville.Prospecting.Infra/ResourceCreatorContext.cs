// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Constructs;
using HashiCorp.Cdktf.Providers.Azurerm.DataAzurermClientConfig;
using HashiCorp.Cdktf.Providers.Azurerm.ResourceGroup;

namespace Softville.Prospecting.Infra;

/// <summary>
/// Generic
/// </summary>
public class ResourceCreatorContext
{
    /// <summary>
    ///
    /// </summary>
    public Construct Scope { get; }

    /// <summary>
    /// Resource group
    /// </summary>
    public ResourceGroup ResourceGroup { get; }
    /// <summary>
    /// Client config
    /// </summary>
    public DataAzurermClientConfig ClientConfig { get; }

    /// <summary>
    /// Admin of the subscription
    /// </summary>
    public string AdminUserObjectId { get; }

    /// <summary>
    ///
    /// </summary>
    public InfrastructureInstance Infrastructure { get; }

    /// <summary>
    /// Tags
    /// </summary>
    public Dictionary<string, string> Tags { get; }

    /// <summary>
    ///
    /// </summary>
    /// <param name="scope"></param>
    /// <param name="resourceGroup"></param>
    /// <param name="clientConfig"></param>
    /// <param name="adminUserObjectId"></param>
    /// <param name="infrastructure"></param>
    /// <param name="tags"></param>
    /// <exception cref="NotImplementedException"></exception>
    public ResourceCreatorContext(Construct scope, ResourceGroup resourceGroup, DataAzurermClientConfig clientConfig,
        string adminUserObjectId, InfrastructureInstance infrastructure, Dictionary<string, string> tags)
    {
        Scope = scope;
        ResourceGroup = resourceGroup;
        ClientConfig = clientConfig;
        AdminUserObjectId = adminUserObjectId;
        Infrastructure = infrastructure;
        Tags = tags;
    }
}
