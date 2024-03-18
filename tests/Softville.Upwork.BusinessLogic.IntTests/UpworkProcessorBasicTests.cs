using FluentAssertions;
using Softville.Upwork.BusinessLogic.IntTests.Infrastructure;
using Softville.Upwork.BusinessLogic.Processor;
using Softville.Upwork.BusinessLogic.Processor.Repositories;
using Softville.Upwork.Contracts;
using Softville.Upwork.Tests.Common.Data;
using WireMock.Matchers;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using Xunit;

namespace Softville.Upwork.BusinessLogic.IntTests;

[Collection(IntPrpCollection.Name)]
public class UpworkProcessorBasicTests(IntPrpContext ctx) : IntTestBase(ctx)
{
    [Fact(DisplayName = "Processed offers from search result saved to db and responses persisted")]
    public async Task GivenOffersInUpworkApi_WhenProcessed_ThenOffersAddedToDbAndRawResponsesPersisted()
    {
        var expectedId = new UpworkId("1745755393334956032", "~019bb7b2a2c6f33572");

        Ctx.Should().NotBeNull();
        var processor = Ctx.Services.GetRequiredService<IUpworkProcessor>();
        processor.Should().NotBeNull();

        Ctx.NetProxy.Server
            .Given(Request
                .Create()
                .WithPath("/ab/jobs/search/url")
                .WithParam("hourly_rate", MatchBehaviour.AcceptOnMatch, "50-")
                .WithParam("page", MatchBehaviour.AcceptOnMatch, "1")
                .WithParam("per_page", MatchBehaviour.AcceptOnMatch, "50")
                .WithParam("q", MatchBehaviour.AcceptOnMatch, ".net")
                .WithParam("sort", MatchBehaviour.AcceptOnMatch, "recency")
                .WithParam("t", MatchBehaviour.AcceptOnMatch, "0")
            )
            .RespondWith(Response
                .Create()
                .WithBody(await TestData.UpworkSearchResultText()));

        Ctx.NetProxy.Server.Given(
            Request
                .Create()
                .WithPath($"/job-details/jobdetails/api/job/{expectedId.CipherText}/summary"))
            .RespondWith(Response
                .Create()
                .WithBody(await TestData.GetCompleteUpworkOfferText()));

        Ctx.NetProxy.Server.Given(
            Request
                .Create()
                .WithPath($"/job-details/jobdetails/api/job/{expectedId.CipherText}/applicants"))
            .RespondWith(Response
                .Create()
                .WithBody(await TestData.UpworkApplicantsText()));

        var mappings = await Ctx.NetProxy.Server.CreateClient().GetAsync("/__admin/mappings");

        // try
        // {
            await processor.ProcessOffersAsync(Ctx.Ct);
        // }
        // catch (Exception e)
        // {
        //     Console.WriteLine(e);
        // }

        var requests = await Ctx.NetProxy.Server.CreateClient().GetAsync("/__admin/requests");

        var offerRepo = Ctx.Services.GetRequiredService<IOfferRepository>();

        var offer = await offerRepo.GetAsync(expectedId, Ctx.Ct);

        offer.Should().NotBeNull();
        offer.Rid.Should().Be(1010624703);
        offer.Title.Should().Be("Lead Sitecore / .Net Engineer");

        Ctx.Services.ResponsePersisting.GetDetails(expectedId).Should().NotBeNull().And
            .Contain("We are looking for Lead Engineer to join our development team");

        Ctx.Services.ResponsePersisting.GetApplicants(expectedId).Should().NotBeNull().And
            .Contain("avgInterviewedRateBid");



    }
}
