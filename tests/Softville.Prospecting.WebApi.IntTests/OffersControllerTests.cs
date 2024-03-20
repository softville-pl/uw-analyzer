using Alba;
using FluentAssertions;
using Softville.Upwork.BusinessLogic.Processor;
using Softville.Upwork.Contracts;
using Softville.Upwork.Tests.Common.Data;
using Softville.Upwork.WebApi.IntTests.Infrastructure;
using WireMock.Matchers;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace Softville.Upwork.WebApi.IntTests;

[Collection(Constants.WebApi)]
public class OffersControllerTests(WebApiContext ctx) : WebApiTestBase(ctx)
{
    [Fact]
    public async Task GivenOffersInDb_WhenGetOffers_ThenOffersReturned()
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

        var scenario = await Ctx.Api.Alba.Scenario(_ =>
        {
            _.Get.Url("/offers");
            _.StatusCodeShouldBeOk();
        });

        var offer = scenario.ReadAsJson<Offer[]>()
            .Should().ContainSingle();

        offer.Subject.Title.Should().Contain("Lead");
    }
}
