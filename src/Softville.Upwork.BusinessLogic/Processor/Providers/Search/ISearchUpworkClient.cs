﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Softville.Upwork.BusinessLogic.Processor.Providers.Search;

internal interface ISearchUpworkClient
{
    public int PageSize { get; }

    Task<UpworkSearchResult> FetchSearchResultPage(int page,
        CancellationToken ct);
}
