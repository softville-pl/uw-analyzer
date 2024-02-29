// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text;
using Microsoft.Extensions.Logging;
using Softville.Upwork.BusinessLogic.Common.Extensions;
using Softville.Upwork.BusinessLogic.Processor.ApplicantsStats;
using Softville.Upwork.BusinessLogic.Processor.Parsers;
using Softville.Upwork.BusinessLogic.Processor.UpworkApi;
using Softville.Upwork.Contracts;

namespace Softville.Upwork.BusinessLogic.Processor;

internal class EndToEndUpworkProcessor(
    ISearchResultProvider searchProvider,
    IApplicantsStatsProvider statsProvider,
    IHttpClientFactory httpClientFactory,
    ILogger<EndToEndUpworkProcessor> logger)
    : IUpworkProcessor
{
    public async Task ProcessOffersAsync(CancellationToken ct)
    {
        List<JobSearch> foundSearchItems = await searchProvider.SearchAsync(ct);

        string rawDetailsOutputFolder =
            @"D:\Sources\Softville\uw-analyzer\src\Softville.Upwork.BusinessLogic\Processor\Data";

        using HttpClient detailsClient = httpClientFactory.CreateClient(UpworkHttpClient.UpworkClientName);

        List<Offer> offers = new(foundSearchItems.Count);
        List<string> problematicOffers = new();
        Contracts.ApplicantsStats? applicantsStats = null;


        OfferSerializer serializer = new();

        string inputFolder = @"D:\Sources\Softville\uw-analyzer\src\Softville.Upwork.BusinessLogic\Processor\Data";
        string outputFolder = @"D:\Sources\Softville\uw-analyzer\src\Softville.Upwork.WebApp\wwwroot\sample-data";

        foreach (JobSearch foundOffer in foundSearchItems)
        {
            var upworkId = new UpworkId(foundOffer.Id, foundOffer.Ciphertext);

            string outputPath = Path.Combine(rawDetailsOutputFolder, $"{upworkId.Uid}.json");

            string offerDetailsContent;

            // if (Path.Exists(outputPath))
            // {
            //     logger.LogWarning("'{@upworkId}' offer details exists. Skipping.", upworkId);
            //
            //     offerDetailsContent = await File.ReadAllTextAsync(outputPath, ct);
            // }
            // else
            {
                var applicantsStatsTask = statsProvider.GetBidsStatsAsync(upworkId, ct);

                var offerTask = detailsClient.GetAsync(
                        $"job-details/jobdetails/api/job/{foundOffer.Ciphertext}/summary", ct);

                await Task.WhenAll([offerTask, applicantsStatsTask]);

                HttpResponseMessage response = await offerTask;
                applicantsStats = await applicantsStatsTask;

                string content = await response.Content.ReadAsStringAsync(ct);

                if (response.IsSuccessStatusCode)
                {
                    logger.LogInformation("'{@upworkId}' request succeeded", upworkId);

                    await File.WriteAllTextAsync(outputPath, content.JsonPrettify(), ct);

                    offerDetailsContent = content;
                }
                else
                {
                    logger.LogError("'{@upworkId}' request failed with status code:{statusCode}. Response: {content}",
                        upworkId, response.StatusCode, content);
                    continue;
                }
            }

            string offerId = foundOffer.Id;

            try
            {
                await using MemoryStream inputOfferFileStream = new(Encoding.UTF8.GetBytes(offerDetailsContent));
                UpworkOffer upworkOffer = await UpworkParser.ParseAsync<UpworkOffer>(inputOfferFileStream, ct);

                Offer offer = upworkOffer.MapToOffer();
                offer.Stats = applicantsStats ?? new Contracts.ApplicantsStats();
                    offer.Requirements = foundOffer.Attrs.Select(attr => attr.PrettyName).ToArray();
                offer.ConnectPrice = foundOffer.ConnectPrice;
                offers.Add(offer);
            }
            catch (Exception e)
            {
                logger.LogError("'{offerId}' couldn't be processed. Reason {reason}", offerId,
                    e.Message + " -> " + e.InnerException?.Message);
                problematicOffers.Add(offerId);
            }
        }

        string problematicOffersOutput = Path.Combine(inputFolder, "problematic-offers.json");

        await using Stream offerStream = await serializer.SerializeAsync(offers, ct);

        await using FileStream outputOfferFileStream =
            new(Path.Join(outputFolder, "offers.json"), FileMode.Truncate);

        await offerStream.CopyToAsync(outputOfferFileStream, ct);

        await outputOfferFileStream.FlushAsync(ct);
        outputOfferFileStream.Close();

        await File.WriteAllTextAsync(problematicOffersOutput, string.Join(Environment.NewLine, problematicOffers), ct);
    }
}
