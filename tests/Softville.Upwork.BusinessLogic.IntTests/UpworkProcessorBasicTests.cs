using FluentAssertions;
using Softville.Upwork.BusinessLogic.IntTests.Infrastructure;
using Xunit;

namespace Softville.Upwork.BusinessLogic.IntTests;

[Collection(IntPrpCollection.Name)]
public class UpworkProcessorBasicTests
{
    private readonly IntPrpContext _ctx;

    public UpworkProcessorBasicTests(IntPrpContext ctx)
    {
        _ctx = ctx;
    }

    [Fact]
    public void Test1()
    {
        _ctx.Should().NotBeNull();
    }
}
