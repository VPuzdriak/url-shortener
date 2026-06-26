using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using UrlShortener.RedirectApi.Infrastructure;
using UrlShortener.RedirectApi.Tests.TestDoubles;

namespace UrlShortener.RedirectApi.Tests;

[Collection("Api collection")]
public sealed class RedirectScenarios(ApiFixture fixture)
{
    private readonly HttpClient _client = fixture.CreateClient(new WebApplicationFactoryClientOptions
    {
        AllowAutoRedirect = false
    });

    private readonly InMemoryShortenedUrlReader _storage = fixture.ShortenedUrlReader;

    [Fact]
    public async Task Should_Return_301_Redirect_With_Url_When_ShortUrl_Exists()
    {
        const string shortUrl = "abc123";
        _storage.Add(shortUrl, new ReadLongUrlResponse(true, "https://www.google.com"));

        var response = await _client.GetAsync($"/r/{shortUrl}");

        response.Should().Be3XXRedirection();
        response.Should().HaveHttpStatusCode(HttpStatusCode.MovedPermanently);
        response.Headers.Location.Should().Be("https://www.google.com");
    }

    [Fact]
    public async Task Should_Return_404_NotFound_When_ShortUrl_Does_Not_Exist()
    {
        const string shortUrl = "non-existing";
        var response = await _client.GetAsync($"/r/{shortUrl}");

        response.Should().HaveHttpStatusCode(HttpStatusCode.NotFound);
    }
}