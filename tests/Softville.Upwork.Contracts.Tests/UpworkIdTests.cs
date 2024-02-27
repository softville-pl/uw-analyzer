using FluentAssertions;

namespace Softville.Upwork.Contracts.Tests;

public class UpworkIdTests
{
    [Theory]
    [InlineData("1752776060163973120", "~01597fef36c54365ec")]
    public void GivenCorrectData_WhenCreateCompositeId_ThenSuccess(string uid, string cipherText)
    {
        var sut = new UpworkId(uid, cipherText);

        sut.Uid.Should().Be(uid);
        sut.CipherText.Should().Be(cipherText);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void GivenEmptyUid_WhenCreateCompositeId_ThenException(string uid)
    {
        Assert.Throws<ArgumentException>(() => new UpworkId(uid, "~01597fef36c54365ec")).Should()
            .BeOfType<ArgumentException>().Which.Message.Should().Contain("The value cannot be an empty string or composed entirely of whitespace. (Parameter 'uid')");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void GivenEmptyCipherText_WhenCreateCompositeId_ThenException(string cipherText)
    {
        Assert.Throws<ArgumentException>(() => new UpworkId("1752776060163973120", cipherText)).Should()
            .BeOfType<ArgumentException>().Which.Message.Should().Contain("The value cannot be an empty string or composed entirely of whitespace. (Parameter 'cipherText')");
    }

    [Theory]
    [InlineData("01597fef36c54365ec")]
    public void GivenIncorrectSyntaxCipherText_WhenCreateCompositeId_ThenException(string cipherText)
    {
        Assert.Throws<ArgumentException>(() => new UpworkId("1752776060163973120", cipherText)).Should()
            .BeOfType<ArgumentException>().Which.Message.Should().Contain($"cipherText must starts with '~'. Actual value: {cipherText} (Parameter 'cipherText')");
    }
}
