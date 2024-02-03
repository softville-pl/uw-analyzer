using Softville.Upwork.BusinessLogic.Tests.Processor.TestData;

namespace Softville.Upwork.BusinessLogic.Tests.Processor.UpworkParser;

public class UpworkParserTests
{
    [Fact(DisplayName = "Complete Upwork data parsed successfully")]
    public async Task GivenCompleteUpworkJsonData_WhenParse_ThenParsed()
    {
        await using Stream stream = ProcessorTestData.GetCompleteUpworkOffer();

        UpworkOffer actual =
            await new BusinessLogic.Processor.UpworkParser().ParseAsync(stream, CancellationToken.None);

        VerifySettings settings = new();
        settings.DontScrubDateTimes();

        await Verify(actual, settings);
    }
}
