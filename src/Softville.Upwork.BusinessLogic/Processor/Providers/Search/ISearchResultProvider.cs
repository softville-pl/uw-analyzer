// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Softville.Upwork.BusinessLogic.Processor.Providers.Search;

internal interface ISearchResultProvider
{
    Task<List<JobSearch>> SearchAsync(CancellationToken ct);
}
