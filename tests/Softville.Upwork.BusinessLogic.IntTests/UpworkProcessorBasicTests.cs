using FluentAssertions;
using Softville.Upwork.BusinessLogic.IntTests.Infrastructure;
using Softville.Upwork.BusinessLogic.Processor;
using Softville.Upwork.Tests.Common.Data;
using WireMock.Matchers;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using Xunit;

namespace Softville.Upwork.BusinessLogic.IntTests;

[Collection(IntPrpCollection.Name)]
public class UpworkProcessorBasicTests(IntPrpContext ctx) : IntTestBase(ctx)
{
    [Fact]
    public async Task GivenDIContainer_WhenGetUpworkProcessor_ThenSuccess()
    {
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
                .WithPath($"/job-details/jobdetails/api/job/~019bb7b2a2c6f33572/summary"))
            .RespondWith(Response
                .Create()
                .WithBody(await TestData.GetCompleteUpworkOfferText()));

        Ctx.NetProxy.Server.Given(
            Request
                .Create()
                .WithPath($"/job-details/jobdetails/api/job/~019bb7b2a2c6f33572/applicants"))
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


    }
}
