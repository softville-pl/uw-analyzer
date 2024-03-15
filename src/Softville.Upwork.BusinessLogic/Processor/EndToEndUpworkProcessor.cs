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
        using HttpClient detailsClient = httpClientFactory.CreateClient(UpworkHttpClient.UpworkClientName);

        List<JobSearch> foundSearchItems = await searchProvider.SearchAsync(ct);

        List<Offer> offers = new(foundSearchItems.Count);
        List<string> problematicOffers = new();

        OfferSerializer serializer = new();

        foreach (JobSearch foundOffer in foundSearchItems)
        {
            var upworkId = new UpworkId(foundOffer.Id, foundOffer.Ciphertext);

            Contracts.ApplicantsStats? applicantsStats;
            Offer? offer;
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

        string outputFolder = @"D:\Sources\Softville\uw-analyzer\src\Softville.Upwork.WebApp\wwwroot\sample-data";

        await using Stream offerStream = await serializer.SerializeAsync(offers, ct);

        await using FileStream outputOfferFileStream =
            new(Path.Join(outputFolder, "offers.json"), FileMode.Truncate);

        await offerStream.CopyToAsync(outputOfferFileStream, ct);

        await outputOfferFileStream.FlushAsync(ct);
        outputOfferFileStream.Close();

        logger.LogWarning(
            $"{problematicOffers.Count} problematic offers found: {string.Join(Environment.NewLine, problematicOffers)}");
    }
}
