// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.Logging;
using Softville.Upwork.BusinessLogic.Processor.ApplicantsStats;
using Softville.Upwork.BusinessLogic.Processor.OfferDetails;
using Softville.Upwork.BusinessLogic.Processor.Repositories;
using Softville.Upwork.BusinessLogic.Processor.UpworkApi;
using Softville.Upwork.Contracts;

namespace Softville.Upwork.BusinessLogic.Processor;

internal class EndToEndUpworkProcessor(
    ISearchResultProvider searchProvider,
    IOfferDetailsProvider detailsProvider,
    IApplicantsStatsProvider statsProvider,
    IHttpClientFactory httpClientFactory,
    IOfferProcessorRepository offerRepository,
    ILogger<EndToEndUpworkProcessor> logger)
    : IUpworkProcessor
{
    public async Task ProcessOffersAsync(CancellationToken ct)
    {
        using HttpClient detailsClient = httpClientFactory.CreateClient(UpworkHttpClient.UpworkClientName);

        List<JobSearch> foundSearchItems = await searchProvider.SearchAsync(ct);

        List<Offer> offers = new(foundSearchItems.Count);
        List<string> problematicOffers = new();

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

            await offerRepository.SaveAsync(offer, ct);
        }

        logger.LogInformation("{processedCount} offers successfully processed", offers.Count);

        logger.LogWarning(
            "{problematicCount} problematic offers found: {problematicId}", problematicOffers.Count, string.Join(Environment.NewLine, problematicOffers));
    }
}
