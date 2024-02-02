using FluentAssertions;
using Softville.Upwork.BusinessLogic.Tests.Infrastructure;
using Xunit;

namespace Softville.Upwork.BusinessLogic.Tests.Processor.UpworkParser;

public class UpworkParserTests
{
    [Fact(DisplayName = "Complete Upwork data parsed successfully")]
    public async Task GivenCompleteUpworkJsonData_WhenParse_ThenParsed()
    {
        await using var stream = GetType().Assembly.GetResourceStream("upwork-fulldatamodel.json");
        var actual = await new BusinessLogic.Processor.UpworkParser().ParseAsync(stream, CancellationToken.None);

        actual.Should().NotBeNull();

        true.Should().BeTrue();
    }
}
