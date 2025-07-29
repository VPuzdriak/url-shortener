using UrlShortener.Code;

namespace UrlShortener.Api.Core.Tests;

public class Base62EncodingScenarios
{
    [Theory]
    [InlineData(0, "0")]
    [InlineData(1, "1")]
    [InlineData(20, "K")]
    [InlineData(987654321, "14q60P")]
    public void Should_Encode_Number_To_Base62(int number, string expected)
    {
        number.EncodeToBase62()
            .Should()
            .Be(expected);
    }
}