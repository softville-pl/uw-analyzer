// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Reflection;

namespace Softville.Upwork.Tests.Common.Data;

internal static class ResourceHelper
{
    internal static Stream GetResourceStream(this Assembly assembly, string resourceName)
    {
        string[] resources = assembly.GetManifestResourceNames();

        string? foundResource = resources.FirstOrDefault(name => name.EndsWith(resourceName));
        if (foundResource == null)
        {
            throw new ArgumentException(
                $"Resource that ends on '{resourceName}' with doesn't exist in '{assembly.GetName()}'. Found resources: {string.Join(", ", resources)}");
        }

        return assembly.GetManifestResourceStream(foundResource)!;
    }

    internal static async Task<string> GetResourceTextAsync(this Assembly assembly, string resourceName, CancellationToken ct)
    {
        using var sr = new StreamReader(assembly.GetResourceStream(resourceName));

        return await sr.ReadToEndAsync(ct);
    }
}
