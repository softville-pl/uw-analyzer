using Alba;
using Softville.Upwork.WebApi.IntTests.Infrastructure;

namespace Softville.Upwork.WebApi.IntTests;

[Collection(Constants.WebApi)]
public class OffersControllerTests(WebApiContext ctx) : WebApiTestBase(ctx)
{
    [Fact]
    public async Task GivenOffersInDb_WhenGetOffers_ThenOffersReturned()
    {
        var scenario = await Ctx.Api.Alba.Scenario(_ =>
        {
            _.Get.Url("/weatherforecast");
            _.StatusCodeShouldBeOk();
        });
    }
}
