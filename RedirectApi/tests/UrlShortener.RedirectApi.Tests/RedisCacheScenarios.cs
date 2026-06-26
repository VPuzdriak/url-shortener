using NSubstitute;
using StackExchange.Redis;
using UrlShortener.RedirectApi.Infrastructure;

namespace UrlShortener.RedirectApi.Tests;

[Collection(("Api collection"))]
public sealed class RedisCacheScenarios(ApiFixture fixture)
{
    private readonly IConnectionMultiplexer _connectionMultiplexer =
        ConnectionMultiplexer.Connect(fixture.RedisConnectionString);

    [Fact]
    public async Task Should_Get_From_Reader_If_Not_In_Cache()
    {
        var reader = Substitute.For<IShortenedUrlReader>();
        reader.GetLongUrlAsync("short", Arg.Any<CancellationToken>())
            .Returns(new ReadLongUrlResponse(true, "https://google.com"));
        var cache = new RedisUrlReader(reader, _connectionMultiplexer);

        _ = await cache.GetLongUrlAsync("short", CancellationToken.None);

        await reader.Received().GetLongUrlAsync("short", Arg.Any<CancellationToken>());
    }
}