// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using FluentAssertions;
using Softville.Upwork.BusinessLogic.IntTests.Infrastructure;
using Softville.Upwork.BusinessLogic.IntTests.Infrastructure.AutoFixture;
using Softville.Upwork.BusinessLogic.Processor.Repositories;
using Softville.Upwork.Contracts;

namespace Softville.Upwork.BusinessLogic.IntTests;

[Collection(IntPrpCollection.Name)]
public class OfferProcessorRepositoryTests(IntPrpContext ctx) : BusinessLogicTestBase(ctx)
{
    [Theory]
    [BusinessIntTestsAutoData]
    public async Task GivenOffer_WhenSaveAsync_ThenPersistedIntoDb(Offer expected)
    {
        var  offerRepository = Ctx.Job.Services.GetRequiredService<IOfferProcessorRepository>();

        await offerRepository.SaveAsync(expected, Ctx.Ct);

        var actual = await offerRepository.GetAsync(expected.Uid, Ctx.Ct);

        actual.CipherText.Should().Be(expected.CipherText);

        actual.Should().BeEquivalentTo(expected);
    }

    [Theory]
    [BusinessIntTestsAutoData]
    public async Task GivenOffers_WhenGetAllAsync_ThenReturned(Offer[] expectedOffers)
    {
        var  offerRepository = Ctx.Job.Services.GetRequiredService<IOfferProcessorRepository>();

        foreach (Offer expected in expectedOffers)
            await offerRepository.SaveAsync(expected, Ctx.Ct);

        var actualOffers = await offerRepository.GetAllAsync(Ctx.Ct);

        actualOffers.Should().BeEquivalentTo(expectedOffers);
    }
}
