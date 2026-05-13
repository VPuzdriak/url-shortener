using FluentAssertions;
using UrlShortener.Core;

namespace UrlShortener.Api.Core.Tests;

public sealed class ShortUrlGeneratorScenarios
{
    [Fact]
    public void Should_Return_Short_Url_For_10001()
    {
        var tokenProvider = new TokenProvider();
        tokenProvider.AssignRange(10_001, 20_000);
        var shortUrlGenerator = new ShortUrlGenerator(tokenProvider);

        var shortUrl = shortUrlGenerator.GenerateUniqueUrl();

        shortUrl.Should().Be("2bJ");
    }

    [Fact]
    public void Should_Return_Short_Url_For_Zero()
    {
        var tokenProvider = new TokenProvider();
        tokenProvider.AssignRange(0, 10);
        var shortUrlGenerator = new ShortUrlGenerator(tokenProvider);

        var shortUrl = shortUrlGenerator.GenerateUniqueUrl();

        shortUrl.Should().Be("0");
    }
}