// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.Logging;

namespace Softville.Upwork.BusinessLogic.Processor;

internal class UpworkProvider(ILogger<UpworkProvider> logger, IHttpClientFactory httpClientFactory) : IUpworkProvider
{
    public async Task ProvideOffers(CancellationToken ct)
    {
        string outputFolder = @"d:\Misc\Temp\UpworkData\";

        using HttpClient client = httpClientFactory.CreateClient(UpworkHttpClient.Name);

        foreach (UpworkOfferItem offer in UpworkOfferItem.OffersToProcess)
        {
            string outputPath = Path.Combine(outputFolder, $"{offer.Id}.json");

            if (Path.Exists(outputPath))
            {
                logger.LogWarning(@"'{offerId}{cipherText}' offer details exists. Skipping.", offer.Ciphertext,
                    offer.Id);
                continue;
            }

            HttpResponseMessage response =
                await client.GetAsync(
                    $"https://www.upwork.com/job-details/jobdetails/api/job/{offer.Ciphertext}/summary", ct);

            string content = await response.Content.ReadAsStringAsync(ct);

            if (response.IsSuccessStatusCode)
            {
                await File.WriteAllTextAsync(outputPath, content, ct);

                logger.LogInformation("{offerId}{cipherText} request succeeded", offer.Id, offer.Ciphertext);
            }
            else
            {
                logger.LogError(
                    "{offerId}{cipherText} request failed with status code:{statusCode}. Response: {content}",
                    offer.Id, offer.Ciphertext, response.StatusCode, content);
            }
        }
    }
}
