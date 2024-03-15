// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using FluentAssertions;
using Softville.Upwork.BusinessLogic.IntTests.Infrastructure;
using Softville.Upwork.BusinessLogic.IntTests.Infrastructure.AutoFixture;
using Softville.Upwork.BusinessLogic.Processor.Repositories;
using Softville.Upwork.Contracts;
using Xunit;

namespace Softville.Upwork.BusinessLogic.IntTests;

[Collection(IntPrpCollection.Name)]
public class OfferRepositoryTests(IntPrpContext ctx) : IntTestBase(ctx)
{
    [Theory]
    [BusinessIntTestsAutoData]
    public async Task GivenOffer_WhenSaveAsync_ThenPersistedIntoDb(Offer expected)
    {
        var ct = CancellationToken.None;

        var  offerRepository = Ctx.Services.GetRequiredService<IOfferRepository>();

        await offerRepository.SaveAsync(expected, ct);

        var actual = await offerRepository.GetAsync(expected.Uid, ct);

        actual.CipherText.Should().Be(expected.CipherText);

        actual.Should().BeEquivalentTo(expected);
    }
}
