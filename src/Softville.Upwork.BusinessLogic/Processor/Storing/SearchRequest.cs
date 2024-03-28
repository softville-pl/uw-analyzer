// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Softville.Upwork.BusinessLogic.Processor.Storing;

public record SearchRequest : IUpworkRequestType
{
    internal static IUpworkRequestType Instance { get; } = new SearchRequest();

    private SearchRequest()
    { }

    public string RequestName { get; } = "search";
}
