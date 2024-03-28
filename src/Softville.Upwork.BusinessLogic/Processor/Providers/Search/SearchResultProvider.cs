// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.Logging;

namespace Softville.Upwork.BusinessLogic.Processor.Providers.Search;

internal class SearchResultProvider(ISearchUpworkClient searchClient, ILogger<SearchResultProvider> logger)
    : ISearchResultProvider
{
    public async Task<List<JobSearch>> SearchAsync(CancellationToken ct)
    {
        int page = 0;
        int totalCount;
        List<JobSearch> foundOffers = new();

        do
        {
            page++;
            UpworkSearchResult result = await searchClient.FetchSearchResultPage(page, ct);

            foundOffers.AddRange(result.SearchResults.Jobs);

            totalCount = result.SearchResults.Paging.Total;
        } while (page * searchClient.PageSize < totalCount);

        logger.LogInformation("Found '{foundItems}' in total", foundOffers.Count);

        if (Math.Abs(foundOffers.Count - totalCount) > 1)
        {
            logger.LogError("Found less items '{foundCount}' that expected '{totalCount}'", foundOffers.Count,
                totalCount);
        }

        return foundOffers;
    }
}
