// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Softville.Upwork.BusinessLogic.Processor.Storing;

internal record ApplicantsRequest : IUpworkRequestType
{
    private ApplicantsRequest() { }
    internal static IUpworkRequestType Instance { get; } = new ApplicantsRequest();

    public string RequestName { get; } = "applicants";
}
