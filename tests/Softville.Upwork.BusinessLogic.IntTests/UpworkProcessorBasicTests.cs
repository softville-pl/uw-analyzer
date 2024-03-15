using FluentAssertions;
using Softville.Upwork.BusinessLogic.IntTests.Infrastructure;
using Softville.Upwork.BusinessLogic.Processor;
using Xunit;

namespace Softville.Upwork.BusinessLogic.IntTests;

[Collection(IntPrpCollection.Name)]
public class UpworkProcessorBasicTests(IntPrpContext ctx) : IntTestBase(ctx)
{
    [Fact]
    public void GivenDIContainer_WhenGetUpworkProcessor_ThenSuccess()
    {
        Ctx.Should().NotBeNull();
        var processor = Ctx.Services.GetRequiredService<IUpworkProcessor>();
        processor.Should().NotBeNull();

        var connString =  Ctx.Database.ConnectionString;

        connString.Should().NotBeEmpty();
    }
}
