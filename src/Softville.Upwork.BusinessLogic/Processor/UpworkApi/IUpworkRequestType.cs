﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Softville.Upwork.BusinessLogic.Processor.UpworkApi;

public interface IUpworkRequestType
{
    public string RequestName { get; }
}

internal record ApplicantsRequest : IUpworkRequestType
{
    private ApplicantsRequest() { }
    internal static IUpworkRequestType Instance { get; } = new ApplicantsRequest();

    public string RequestName { get; } = "applicants";
}

internal record DetailsRequest : IUpworkRequestType
{
    private DetailsRequest() { }
    internal static IUpworkRequestType Instance { get; } = new DetailsRequest();

    public string RequestName { get; } = string.Empty;
}
