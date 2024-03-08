// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Softville.Prospecting.Infra;

/// <summary>
///
/// </summary>
/// <param name="workload"></param>
/// <param name="environment"></param>
/// <param name="instance"></param>
public class InfrastructureInstance(string workload, string environment, string instance)
{
    /// <summary>
    ///
    /// </summary>
    public string Workload { get; } = workload;
    /// <summary>
    ///
    /// </summary>
    public string Environment { get; } = environment;
    /// <summary>
    ///
    /// </summary>
    public string Instance { get; } = instance;

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public string GetResourceNamePostfix() => $"{Workload}-{Environment}-plc-{Instance}";
    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public string GetStorageAccountNameNamePostfix() => $"{Workload}{Environment}plc{Instance}";
}
