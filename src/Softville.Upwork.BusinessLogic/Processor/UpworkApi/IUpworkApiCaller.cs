// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Softville.Upwork.BusinessLogic.Processor.UpworkApi;

internal interface IUpworkApiCaller
{
    Task<HttpResponseMessage> SendRequestAsync(HttpRequestMessage request, CancellationToken ct);
}
