// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Softville.Upwork.BusinessLogic.Processor.Storing;

internal record DetailsRequest : IUpworkRequestType
{
    private DetailsRequest() { }
    internal static IUpworkRequestType Instance { get; } = new DetailsRequest();

    public string RequestName { get; } = string.Empty;
}
