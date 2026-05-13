using FluentAssertions;
using UrlShortener.Core;

namespace UrlShortener.Api.Core.Tests;

public sealed class Base62EncodingScenarios
{
    [Theory]
    [InlineData(0, "0")]
    [InlineData(20, "K")]
    [InlineData(61, "z")]
    [InlineData(1000, "G8")]
    [InlineData(987654321, "14q60P")]
    public void Should_Encode_Number_To_Base62(long number, string expected)
    {
        number.EncodeToBase62().Should().Be(expected);
    }
}