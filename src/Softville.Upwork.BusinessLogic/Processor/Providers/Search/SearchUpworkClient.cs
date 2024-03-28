// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Net;
using Microsoft.Extensions.Logging;
using Softville.Upwork.BusinessLogic.Processor.Parsers;
using Softville.Upwork.BusinessLogic.Processor.UpworkApi;

namespace Softville.Upwork.BusinessLogic.Processor.Providers.Search;

internal class SearchUpworkClient(IHttpClientFactory httpClientFactory, ILogger<SearchUpworkClient> logger) : ISearchUpworkClient
{
    public int PageSize { get; } = 50;

    public async Task<UpworkSearchResult> FetchSearchResultPage(int page,
        CancellationToken ct)
    {
        using HttpClient httpClient = httpClientFactory.CreateClient(UpworkHttpClient.UpworkClientName);

        logger.LogInformation("Fetching #{page} page for {pageSize} items", page, PageSize);

        HttpResponseMessage offersSearchResponse = await httpClient.GetAsync(
            GetSearchQuery(page, PageSize),
            ct);

        if (!offersSearchResponse.IsSuccessStatusCode)
        {
            throw new WebException(
                $"Failed to query Upwork search. Response: {await offersSearchResponse.Content.ReadAsStringAsync(ct)}");
        }

        Stream responseStream = await offersSearchResponse.Content.ReadAsStreamAsync(ct);

        UpworkSearchResult result = await UpworkSearchResultParser.ParseSearchResults(
            responseStream,
            ct);

        logger.LogDebug("Fetch #{page} page with {foundItems} items", page, result.SearchResults.Jobs.Count);

        return result;
    }

    private static string GetSearchQuery(int page, int pageSize) =>
        $"ab/jobs/search/url?hourly_rate=50-&page={page}&per_page={pageSize}&q=.net&sort=recency&t=0";
}
