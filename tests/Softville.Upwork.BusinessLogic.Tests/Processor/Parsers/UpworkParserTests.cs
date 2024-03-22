using Softville.Upwork.BusinessLogic.Processor.Parsers;
using Softville.Upwork.Tests.Common.Data;

namespace Softville.Upwork.BusinessLogic.Tests.Processor.Parsers;

public class UpworkParserTests
{
    [Fact(DisplayName = "Complete Upwork data parsed successfully")]
    public async Task GivenCompleteUpworkJsonData_WhenParse_ThenParsed()
    {
        await using Stream stream = TestData.Offer1DetailsV1.GetStream();

        UpworkOffer actual = await UpworkParser.ParseAsync<UpworkOffer>(stream, CancellationToken.None);

        VerifySettings settings = new();
        settings.DontScrubDateTimes();

        await Verify(actual, settings);
    }
}
