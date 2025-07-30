using UrlShortener.Core;
using UrlShortener.Core.Urls.Add;

namespace UrlShortener.Api.Core.Tests.Urls;

public class ShortUrlGeneratorScenarios
{
    [Fact]
    public void Should_Return_Short_Url_For_Zero()
    {
        var tokenProvider = new TokenProvider();
        tokenProvider.AssignRange(0, 10000);
        var shortUrlGenerator = new ShortUrlGenerator(tokenProvider);

        var shortUrl = shortUrlGenerator.GenerateUniqueUrl();

        shortUrl.Should().Be("0");
    }

    [Fact]
    public void Should_Return_Short_Url_For_10001()
    {
        var tokenProvider = new TokenProvider();
        tokenProvider.AssignRange(10001, 20000);
        var shortUrlGenerator = new ShortUrlGenerator(tokenProvider);

        var shortUrl = shortUrlGenerator.GenerateUniqueUrl();

        shortUrl.Should().Be("2bJ");
    }
}