using FluentAssertions;
using Softville.Upwork.BusinessLogic.IntTests.Infrastructure;
using Softville.Upwork.BusinessLogic.Processor;
using Softville.Upwork.BusinessLogic.Processor.Repositories;
using Softville.Upwork.Contracts;
using Softville.Upwork.Tests.Common.Data;
using WireMock.Matchers;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace Softville.Upwork.BusinessLogic.IntTests;

[Collection(IntPrpCollection.Name)]
public class UpworkProcessorBasicTests(IntPrpContext ctx) : BusinessLogicTestBase(ctx)
{
    [Fact(DisplayName = "New offers from search result saved to db and responses persisted during processing")]
    public async Task GivenOfferInUpworkApi_WhenProcessed_ThenOffersAddedToDbAndRawResponsesPersisted()
    {
        var expectedId = new UpworkId("1745755393334956032", "~019bb7b2a2c6f33572");

        Ctx.Should().NotBeNull();
        var processor = Ctx.Job.Services.GetRequiredService<IUpworkProcessor>();
        processor.Should().NotBeNull();

        Ctx.NetProxy.Server
            .Given(Request.Create()
                .WithPath("/ab/jobs/search/url")
                .WithParam("hourly_rate", MatchBehaviour.AcceptOnMatch, "50-")
                .WithParam("page", MatchBehaviour.AcceptOnMatch, "1")
                .WithParam("per_page", MatchBehaviour.AcceptOnMatch, "50")
                .WithParam("q", MatchBehaviour.AcceptOnMatch, ".net")
                .WithParam("sort", MatchBehaviour.AcceptOnMatch, "recency")
                .WithParam("t", MatchBehaviour.AcceptOnMatch, "0"))
            .RespondWith(Response
                .Create()
                .WithBody(await TestData.UpworkSearchResultText()));

        Ctx.NetProxy.Server
            .Given(Request.Create().WithPath($"/job-details/jobdetails/api/job/{expectedId.CipherText}/summary"))
            .RespondWith(Response.Create().WithBody(await TestData.GetCompleteUpworkOfferText()));

        Ctx.NetProxy.Server
            .Given(Request.Create().WithPath($"/job-details/jobdetails/api/job/{expectedId.CipherText}/applicants"))
            .RespondWith(Response.Create().WithBody(await TestData.UpworkApplicantsText()));

        await processor.ProcessOffersAsync(Ctx.Ct);

        var offerRepo = Ctx.Job.Services.GetRequiredService<IOfferProcessorRepository>();

        var offer = await offerRepo.GetAsync(expectedId, Ctx.Ct);

        await Verify(offer, Ctx.Verify.CreateSettings());

        Ctx.Job.Services.ResponsePersisting.GetDetails(expectedId).Should().NotBeNull().And
            .Contain("We are looking for Lead Engineer to join our development team");

        Ctx.Job.Services.ResponsePersisting.GetApplicants(expectedId).Should().NotBeNull().And
            .Contain("avgInterviewedRateBid");
    }

    [Fact(DisplayName = "Updated offers from search result updated in db and responses persisted during processing")]
    public async Task GivenOfferInDbAndTheNewVersionInUpworkApi_WhenProcessed_ThenOfferInDbAndRawResponsesUpdated()
    {
        var expectedId = new UpworkId("1745755393334956032", "~019bb7b2a2c6f33572");
        var offerRepo = Ctx.Job.Services.GetRequiredService<IOfferProcessorRepository>();

        Ctx.Should().NotBeNull();
        var processor = Ctx.Job.Services.GetRequiredService<IUpworkProcessor>();
        processor.Should().NotBeNull();

        Ctx.NetProxy.Server
            .Given(Request.Create()
                .WithPath("/ab/jobs/search/url")
                .WithParam("hourly_rate", MatchBehaviour.AcceptOnMatch, "50-")
                .WithParam("page", MatchBehaviour.AcceptOnMatch, "1")
                .WithParam("per_page", MatchBehaviour.AcceptOnMatch, "50")
                .WithParam("q", MatchBehaviour.AcceptOnMatch, ".net")
                .WithParam("sort", MatchBehaviour.AcceptOnMatch, "recency")
                .WithParam("t", MatchBehaviour.AcceptOnMatch, "0"))
            .RespondWith(Response
                .Create()
                .WithBody(await TestData.UpworkSearchResultText()));

        Ctx.NetProxy.Server
            .Given(Request.Create().WithPath($"/job-details/jobdetails/api/job/{expectedId.CipherText}/summary"))
            .RespondWith(Response.Create().WithBody(await TestData.GetCompleteUpworkOfferText()));

        Ctx.NetProxy.Server
            .Given(Request.Create().WithPath($"/job-details/jobdetails/api/job/{expectedId.CipherText}/applicants"))
            .RespondWith(Response.Create().WithBody(await TestData.UpworkApplicantsText()));

        (await offerRepo.GetAsync(expectedId, Ctx.Ct)).Should().BeNull();

        await processor.ProcessOffersAsync(Ctx.Ct);

        (await offerRepo.GetAsync(expectedId, Ctx.Ct)).Should().NotBeNull();

        Ctx.NetProxy.Server
            .Given(Request.Create().WithPath($"/job-details/jobdetails/api/job/{expectedId.CipherText}/summary"))
            .RespondWith(Response.Create().WithBody(await TestData.GetCompleteUpworkOfferV2Text()));

        Ctx.NetProxy.Server
            .Given(Request.Create().WithPath($"/job-details/jobdetails/api/job/{expectedId.CipherText}/applicants"))
            .RespondWith(Response.Create().WithBody(await TestData.UpworkApplicantsV2Text()));

        await processor.ProcessOffersAsync(Ctx.Ct);

        var offer = await offerRepo.GetAsync(expectedId, Ctx.Ct);

        await Verify(offer, Ctx.Verify.CreateSettings());

        Ctx.Job.Services.ResponsePersisting.GetDetails(expectedId).Should().NotBeNull().And
            .Contain("New description");

        Ctx.Job.Services.ResponsePersisting.GetApplicants(expectedId).Should().NotBeNull().And
            .Contain("900");
    }
}
