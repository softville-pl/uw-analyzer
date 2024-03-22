// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Softville.Upwork.Contracts;

namespace Softville.Upwork.Tests.Common.Data;

public class ResourceOffer(UpworkId id, string resourceName)
{
    public UpworkId Id { get; } = id;

    public Stream GetStream() =>
        typeof(TestData).Assembly.GetResourceStream(resourceName);

    public Task<string> GetText() =>
        typeof(TestData).Assembly.GetResourceTextAsync(resourceName, CancellationToken.None);


}
