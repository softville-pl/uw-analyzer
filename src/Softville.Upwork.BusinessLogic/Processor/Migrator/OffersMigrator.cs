// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.Logging;
using Softville.Upwork.BusinessLogic.Processor.Models;
using Softville.Upwork.BusinessLogic.Processor.Parsers;
using Softville.Upwork.BusinessLogic.Processor.Providers.ApplicantsStats;
using Softville.Upwork.BusinessLogic.Processor.Repositories;
using Softville.Upwork.BusinessLogic.Processor.Storing;

namespace Softville.Upwork.BusinessLogic.Processor.Migrator;

internal class OffersMigrator(
    IHttpResponseStoring responseStoring,
    IOfferProcessorRepository offersRepo,
    ILogger<OffersMigrator> logger) : IOffersMigrator
{
    public async Task MigrateAsync(CancellationToken ct)
    {
        var ids = await responseStoring.ListAllAsync(ct);

        logger.LogInformation("{offersCount} offers found to migrate", ids.Length);

        await Parallel.ForEachAsync(ids, ct, async (id, ct1) =>
        {
            if ((await offersRepo.GetAsync(id, ct1))?.Uid == id.Uid)
            {
                logger.LogWarning("{id} exists. Skipping", id);
                return;
            }

            logger.LogInformation("{offerId} started migrating", id);
            await using var detailsStream = responseStoring.Read(id, DetailsRequest.Instance);
            if (detailsStream is null)
            {
                logger.LogError("{offerId} cannot be migrated.No details persisted properly", id);
                return;
            }

            var upworkOffer = await UpworkParser.ParseAsync<UpworkOffer>(detailsStream, ct1);
            var offer = upworkOffer.MapToOffer();

            await using var applicantsStream = responseStoring.Read(id, ApplicantsRequest.Instance);
            if (applicantsStream is not null)
                offer.Stats = (await UpworkParser.ParseAsync<UpworkApplicantsStats>(applicantsStream, ct1))
                    .MapToStats();

            offer.Requirements = ["N/A"];
            offer.ConnectPrice = -1;

            await offersRepo.SaveAsync(offer, ct1);

            logger.LogInformation("{id} successfully migrated", id);
        });
    }
}
