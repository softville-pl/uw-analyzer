// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using FluentAssertions;
using Softville.Upwork.BusinessLogic.IntTests.Infrastructure;
using Softville.Upwork.BusinessLogic.Processor.Migrator;
using Softville.Upwork.BusinessLogic.Processor.Repositories;
using Softville.Upwork.Tests.Common.Data;

namespace Softville.Upwork.BusinessLogic.IntTests;

[Collection(IntPrpCollection.Name)]
public class OffersMigratorTests(IntPrpContext ctx) : BusinessLogicTestBase(ctx)
{
    [Fact(DisplayName = "Persisted responses migrated to offers and added to db")]
    public async Task GivenOfferResponsesPersistedAndNoOfferInDb_WhenMigrate_ThenOfferWithApplicantsAddedToDb()
    {
        await Ctx.Job.Services.ResponseStoring.AddDetailsAsync(TestData.Offer1DetailsV1);
        await Ctx.Job.Services.ResponseStoring.AddApplicantsAsync(TestData.Offer1ApplicantsV1);
        var offerRepo = Ctx.Job.Services.GetRequiredService<IOfferProcessorRepository>();

        (await offerRepo.GetAllAsync(Ctx.Ct)).Should().BeEmpty();

        await Ctx.Job.Services.GetRequiredService<IOffersMigrator>().MigrateAsync(Ctx.Ct);

        var actualOffer = (await offerRepo.GetAllAsync(Ctx.Ct)).Should().ContainSingle().Subject;
        await Verify(actualOffer, Ctx.Verify.CreateSettings());
    }
}
