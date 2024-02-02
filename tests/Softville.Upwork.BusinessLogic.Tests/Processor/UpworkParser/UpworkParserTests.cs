using Softville.Upwork.BusinessLogic.Tests.Infrastructure;

namespace Softville.Upwork.BusinessLogic.Tests.Processor.UpworkParser;

public class UpworkParserTests
{
    [Fact(DisplayName = "Complete Upwork data parsed successfully")]
    public async Task GivenCompleteUpworkJsonData_WhenParse_ThenParsed()
    {
        await using Stream stream = GetType().Assembly.GetResourceStream("upwork-fulldatamodel.json");
        UpworkOffer? actual =
            await new BusinessLogic.Processor.UpworkParser().ParseAsync(stream, CancellationToken.None);

        VerifySettings settings = new();
        settings.DontScrubDateTimes();

        await Verify(actual, settings);
    }
}
