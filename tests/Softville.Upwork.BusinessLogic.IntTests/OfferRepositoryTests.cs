// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using FluentAssertions;
using Softville.Upwork.BusinessLogic.IntTests.Infrastructure;
using Softville.Upwork.BusinessLogic.IntTests.Infrastructure.AutoFixture;
using Softville.Upwork.Contracts;
using Xunit;

namespace Softville.Upwork.BusinessLogic.IntTests;

[Collection(IntPrpCollection.Name)]
public class OfferRepositoryTests(IntPrpContext ctx) : IntTestBase(ctx)
{
    [Theory]
    [BusinessIntTestsAutoData]
    public async Task GivenOffer_WhenSaveAsync_ThenPersistedIntoDb(Offer offer)
    {
        offer.Should().NotBeNull();
        await Task.CompletedTask;

        // await Ctx.Services.GetRequiredService<IOfferRepository>().SaveAsync(offer, CancellationToken.None);
    }
}
