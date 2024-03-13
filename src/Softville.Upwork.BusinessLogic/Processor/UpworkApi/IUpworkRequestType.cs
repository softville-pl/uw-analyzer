// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Softville.Upwork.BusinessLogic.Processor.UpworkApi;

public interface IUpworkRequestType
{
    public string RequestName { get; }
}

internal class ApplicantsRequest : IUpworkRequestType
{
    internal static readonly IUpworkRequestType Instance = new ApplicantsRequest();

    public string RequestName { get; } = "applicants";
}

internal class DetailsRequest : IUpworkRequestType
{
    internal static readonly IUpworkRequestType Instance = new DetailsRequest();

    public string RequestName { get; } = string.Empty;
}
