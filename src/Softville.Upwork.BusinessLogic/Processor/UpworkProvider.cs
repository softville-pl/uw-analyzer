// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Net;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using Softville.Upwork.BusinessLogic.Common.Extensions;

namespace Softville.Upwork.BusinessLogic.Processor;

internal class UpworkProvider(ILogger<UpworkProvider> logger, IHttpClientFactory httpClientFactory) : IUpworkProvider
{
    public async Task ProvideOffers(CancellationToken ct)
    {
        string outputFolder = @"d:\Misc\Temp\UpworkData\";

        using HttpClient detailsClient = httpClientFactory.CreateClient(UpworkHttpClient.DetailsClientName);


        HttpResponseMessage offersSearchResponse = await detailsClient.GetAsync(
            "https://www.upwork.com/ab/jobs/search/url?contractor_tier=3&hourly_rate=50-&page=1&per_page=50&q=.net&sort=recency&t=0",
            ct);

        string offersSearchContent = await offersSearchResponse.Content.ReadAsStringAsync(ct);

        if (!offersSearchResponse.IsSuccessStatusCode)
        {
            throw new WebException($"Failed to query Upwork search. Response: {offersSearchContent}");
        }

        System.Text.RegularExpressions.Match match = Regex.Match(offersSearchContent, @"~[a-zA-Z0-9]{18}/""",
            RegexOptions.Compiled);
        if (!match.Success)
        {
            throw new ArgumentException("Upwork search result doesn't contain any offer ciphered text");
        }

        foreach (Group matchGroup in match.Groups)
        {
            string cipherText = matchGroup.Value.Substring(1, 19);

            string outputPath = Path.Combine(outputFolder, $"{cipherText}.json");

            if (Path.Exists(outputPath))
            {
                logger.LogWarning(@"'{cipherText}' offer details exists. Skipping.", cipherText);
                continue;
            }

            HttpResponseMessage response =
                await detailsClient.GetAsync(
                    $"https://www.upwork.com/job-details/jobdetails/api/job/~{cipherText}/summary", ct);

            string content = await response.Content.ReadAsStringAsync(ct);

            if (response.IsSuccessStatusCode)
            {
                await File.WriteAllTextAsync(outputPath, content.JsonPrettify(), ct);

                logger.LogInformation("{cipherText} request succeeded", cipherText);
            }
            else
            {
                logger.LogError(
                    "{cipherText} request failed with status code:{statusCode}. Response: {content}",
                    cipherText, response.StatusCode, content);
            }
        }


        // using HttpClient detailsClient = httpClientFactory.CreateClient(UpworkHttpClient.DetailsClientName);
        //
        // foreach (UpworkOfferItem offer in UpworkOfferItem.OffersToProcess)
        // {
        //     string outputPath = Path.Combine(outputFolder, $"{offer.Id}.json");
        //
        //     if (Path.Exists(outputPath))
        //     {
        //         logger.LogWarning(@"'{offerId}{cipherText}' offer details exists. Skipping.", offer.Ciphertext,
        //             offer.Id);
        //         continue;
        //     }
        //
        //     HttpResponseMessage response =
        //         await detailsClient.GetAsync(
        //             $"https://www.upwork.com/job-details/jobdetails/api/job/{offer.Ciphertext}/summary", ct);
        //
        //     string content = await response.Content.ReadAsStringAsync(ct);
        //
        //     if (response.IsSuccessStatusCode)
        //     {
        //         await File.WriteAllTextAsync(outputPath, content, ct);
        //
        //         logger.LogInformation("{offerId}{cipherText} request succeeded", offer.Id, offer.Ciphertext);
        //     }
        //     else
        //     {
        //         logger.LogError(
        //             "{offerId}{cipherText} request failed with status code:{statusCode}. Response: {content}",
        //             offer.Id, offer.Ciphertext, response.StatusCode, content);
        //     }
        // }
    }
}
