using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Softville.Upwork.BusinessLogic.IntTests.Infrastructure;
using Softville.Upwork.BusinessLogic.Processor;
using Xunit;

namespace Softville.Upwork.BusinessLogic.IntTests;

[Collection(IntPrpCollection.Name)]
public class UpworkProcessorBasicTests(IntPrpContext ctx) : IntTestBase(ctx)
{
    [Fact]
    public void Test1()
    {
        Ctx.Should().NotBeNull();
        Ctx.Services.GetRequiredService<IUpworkProcessor>().Should().NotBeNull();
    }
}
