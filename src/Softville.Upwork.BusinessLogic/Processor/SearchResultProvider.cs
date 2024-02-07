// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Net;
using Microsoft.Extensions.Logging;
using Softville.Upwork.BusinessLogic.Processor.Parsers;

namespace Softville.Upwork.BusinessLogic.Processor;

internal class SearchResultProvider(ILogger<SearchResultProvider> logger, IHttpClientFactory httpClientFactory)
    : ISearchResultProvider
{
    private const int PageSize = 50;

    public async Task<List<JobSearch>> SearchAsync(CancellationToken ct)
    {
        using HttpClient httpClient = httpClientFactory.CreateClient(UpworkHttpClient.DetailsClientName);

        int page = 0;
        int totalCount;
        List<JobSearch> foundOffers = new();

        do
        {
            page++;
            UpworkSearchResult result = await FetchSearchResultPage(httpClient, page, ct);

            foundOffers.AddRange(result.SearchResults.Jobs);

            totalCount = result.SearchResults.Paging.Total;
        } while (page * PageSize < totalCount);

        logger.LogInformation("Found '{foundItems}' in total", foundOffers.Count);

        if (Math.Abs(foundOffers.Count - totalCount) > 1)
        {
            throw new InvalidOperationException($"Found less items '{foundOffers.Count}' that expected '{totalCount}'");
        }

        return foundOffers;
    }

    private async Task<UpworkSearchResult> FetchSearchResultPage(HttpClient httpClient, int page,
        CancellationToken ct)
    {
        int pageSize = PageSize;
        logger.LogInformation("Fetching #{page} page for {pageSize} items", page, pageSize);

        HttpResponseMessage offersSearchResponse = await httpClient.GetAsync(
            GetSearchQuery(page, pageSize),
            ct);

        if (!offersSearchResponse.IsSuccessStatusCode)
        {
            throw new WebException(
                $"Failed to query Upwork search. Response: {await offersSearchResponse.Content.ReadAsStringAsync(ct)}");
        }

        UpworkSearchResult result = await UpworkSearchResultParser.ParseSearchResults(
            await offersSearchResponse.Content.ReadAsStreamAsync(ct),
            ct);

        logger.LogDebug("Fetch #{page} page with {foundItems} items", page, result.SearchResults.Jobs.Count);

        return result;
    }

    private static string GetSearchQuery(int page, int pageSize) =>
        $"https://www.upwork.com/ab/jobs/search/url?contractor_tier=3&hourly_rate=50-&page={page}&per_page={pageSize}&q=.net&sort=recency&t=0";
}
