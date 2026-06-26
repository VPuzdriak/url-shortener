using Microsoft.AspNetCore.Mvc.Testing;
using UrlShortener.RedirectApi.Tests.TestDoubles;

namespace UrlShortener.RedirectApi.Tests;

[CollectionDefinition("Api collection")]
public sealed class ApiCollection(ApiFixture fixture) : IClassFixture<ApiFixture>
{
    private readonly HttpClient _client = fixture.CreateClient(new WebApplicationFactoryClientOptions
    {
        AllowAutoRedirect = false
    });

    private readonly InMemoryShortenedUrlReader _storage = fixture.ShortenedUrlReader;
}