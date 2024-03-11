using FluentAssertions;
using Softville.Upwork.BusinessLogic.IntTests.Infrastructure;
using Xunit;

namespace Softville.Upwork.BusinessLogic.IntTests;

[Collection(IntPrpCollection.Name)]
public class UpworkProcessorBasicTests(IntPrpContext ctx) : IntTestBase(ctx)
{
    [Fact]
    public void Test1()
    {
        Ctx.Should().NotBeNull();
    }
}
