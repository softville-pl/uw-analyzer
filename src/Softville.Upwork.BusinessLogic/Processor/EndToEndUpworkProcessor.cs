// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.Logging;
using Softville.Upwork.BusinessLogic.Processor.ApplicantsStats;
using Softville.Upwork.BusinessLogic.Processor.OfferDetails;
using Softville.Upwork.BusinessLogic.Processor.UpworkApi;
using Softville.Upwork.Contracts;

namespace Softville.Upwork.BusinessLogic.Processor;

internal class EndToEndUpworkProcessor(
    ISearchResultProvider searchProvider,
    IOfferDetailsProvider detailsProvider,
    IApplicantsStatsProvider statsProvider,
    IHttpClientFactory httpClientFactory,
    ILogger<EndToEndUpworkProcessor> logger)
    : IUpworkProcessor
{
    public async Task ProcessOffersAsync(CancellationToken ct)
    {
        List<JobSearch> foundSearchItems = await searchProvider.SearchAsync(ct);

        using HttpClient detailsClient = httpClientFactory.CreateClient(UpworkHttpClient.UpworkClientName);

        List<Offer> offers = new(foundSearchItems.Count);
        List<string> problematicOffers = new();
        Contracts.ApplicantsStats? applicantsStats = null;
        Offer? offer = null;


        OfferSerializer serializer = new();

        foreach (JobSearch foundOffer in foundSearchItems)
        {
            var upworkId = new UpworkId(foundOffer.Id, foundOffer.Ciphertext);

            // if (Path.Exists(outputPath))
            // {
            //     logger.LogWarning("'{@upworkId}' offer details exists. Skipping.", upworkId);
            //
            //     offerDetailsContent = await File.ReadAllTextAsync(outputPath, ct);
            // }
            // else
            {
                var applicantsStatsTask = statsProvider.GetBidsStatsAsync(upworkId, ct);

                var offerTask = detailsProvider.GetDetailsAsync(upworkId, ct);

                await Task.WhenAll([offerTask, applicantsStatsTask]);

                offer = await offerTask;
                applicantsStats = await applicantsStatsTask;

                logger.LogInformation("'{@upworkId}' request succeeded", upworkId);
            }

            string offerId = foundOffer.Id;

            try
            {
                offer.Stats = applicantsStats;
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

        string inputFolder = @"D:\Sources\Softville\uw-analyzer\src\Softville.Upwork.BusinessLogic\Processor\Data";

        string outputFolder = @"D:\Sources\Softville\uw-analyzer\src\Softville.Upwork.WebApp\wwwroot\sample-data";

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
