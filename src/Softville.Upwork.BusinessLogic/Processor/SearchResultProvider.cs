// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Net;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Logging;
using Softville.Upwork.BusinessLogic.Common.Extensions;
using Softville.Upwork.BusinessLogic.Processor.Parsers;
using Softville.Upwork.BusinessLogic.Processor.UpworkApi;

namespace Softville.Upwork.BusinessLogic.Processor;

internal class SearchResultProvider(ILogger<SearchResultProvider> logger, IHttpClientFactory httpClientFactory)
    : ISearchResultProvider
{
    private const int PageSize = 50;

    public async Task<List<JobSearch>> SearchAsync(CancellationToken ct)
    {
        using HttpClient httpClient = httpClientFactory.CreateClient(UpworkHttpClient.UpworkClientName);

        int page = 2418;
        int totalCount;
        List<JobSearch> foundOffers = new();

        do
        {
            page++;
            UpworkSearchResult? result = await FetchSearchResultPage(httpClient, page, ct);

            if (result == null)
            {
                return foundOffers;
            }

            foundOffers.AddRange(result.SearchResults.Jobs);

            totalCount = result.SearchResults.Paging.Total;

            if (page < 2)
            {
                logger.LogInformation("Found '{foundItems}' in total", totalCount);
            }
        } while (page * PageSize < totalCount);

        logger.LogInformation("Found '{foundItems}' in total", foundOffers.Count);

        if (Math.Abs(foundOffers.Count - totalCount) > 1)
        {
            logger.LogError("Found less items '{foundCount}' that expected '{totalCount}'", foundOffers.Count,
                totalCount);
        }

        return foundOffers;
    }

    private async Task<UpworkSearchResult?> FetchSearchResultPage(HttpClient httpClient, int page,
        CancellationToken ct)
    {
        int pageSize = PageSize;
        logger.LogInformation("Fetching #{page} page for {pageSize} items", page, pageSize);

        string searchQuery = GetSearchQuery(page, pageSize);

        HttpResponseMessage offersSearchResponse = await httpClient.GetAsync(
            searchQuery,
            ct);

        if (!offersSearchResponse.IsSuccessStatusCode)
        {
            throw new WebException(
                $"Failed to query Upwork search. Response: {await offersSearchResponse.Content.ReadAsStringAsync(ct)}");
        }

        Stream stream = await offersSearchResponse.Content.ReadAsStreamAsync(ct);

        if (page == 1)
        {
            var fileFull = Path.Join(@"d:\Misc\Temp\UpworkData\SearchResults\2024-03-23\", $"{page}-full-result.json");

            await using FileStream fileStream = File.Open(fileFull, FileMode.CreateNew);
            await stream.CopyToAsync(fileStream, ct);

            await fileStream.FlushAsync(ct);
            fileStream.Close();

            stream.Position = 0;
        }

        var file = Path.Join(@"d:\Misc\Temp\UpworkData\SearchResults\2024-03-23\", $"{page}-result.json");

        var jObject = await JsonNode.ParseAsync(stream, cancellationToken: ct) ??
                      throw new InvalidOperationException($"Incorrect syntaxt of json. Query: `{searchQuery}`");

        var jobs = jObject.AsObject()["searchResults"]!["jobs"]!;

        string jobsJson = jobs.ToJsonString().JsonPrettify();

        if (jobsJson.Contains("~01bae2aa462eb28307"))
            return null;

        await File.WriteAllTextAsync(file, jobsJson, ct);

        stream.Position = 0;


        UpworkSearchResult result = await UpworkSearchResultParser.ParseSearchResults(
            stream,
            ct);

        logger.LogDebug("Fetch #{page} page with {foundItems} items", page, result.SearchResults.Jobs.Count);

        return result;
    }

    private static string GetSearchQuery(int page, int pageSize) =>
        $"ab/jobs/search/url?&page={page}&per_page={pageSize}&sort=recency";
}
