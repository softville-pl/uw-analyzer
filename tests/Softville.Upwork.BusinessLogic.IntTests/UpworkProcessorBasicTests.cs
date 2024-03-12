using FluentAssertions;
using Softville.Upwork.BusinessLogic.IntTests.Infrastructure;
using Softville.Upwork.BusinessLogic.Processor;
using Softville.Upwork.BusinessLogic.Processor.Repositories;
using Xunit;

namespace Softville.Upwork.BusinessLogic.IntTests;

[Collection(IntPrpCollection.Name)]
public class UpworkProcessorBasicTests(IntPrpContext ctx) : IntTestBase(ctx)
{
    [Fact]
    public async Task Test1()
    {
        Ctx.Should().NotBeNull();
        Ctx.Services.GetRequiredService<IUpworkProcessor>().Should().NotBeNull();

        await Ctx.Services.GetRequiredService<IOfferRepository>().ConnectAsync(CancellationToken.None);

        var connString =  Ctx.Database.ConnectionString;

        connString.Should().NotBeEmpty();
    }
}
