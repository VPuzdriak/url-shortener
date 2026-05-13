using FluentAssertions;
using Microsoft.Extensions.Time.Testing;
using UrlShortener.Api.Core.Tests.TestDoubles;
using UrlShortener.Core;
using UrlShortener.Core.Urls.Add;

namespace UrlShortener.Api.Core.Tests.Urls;

public sealed class AddUrlScenarios
{
    private readonly AddUrlHandler _handler;
    private readonly InMemoryUrlDataStore _urlDataStore;
    private readonly FakeTimeProvider _timeProvider;

    public AddUrlScenarios()
    {
        var tokenProvider = new TokenProvider();
        tokenProvider.AssignRange(1, 5);

        var shortUrlGenerator = new ShortUrlGenerator(tokenProvider);
        _timeProvider = new FakeTimeProvider();
        _urlDataStore = new InMemoryUrlDataStore();
        _handler = new AddUrlHandler(shortUrlGenerator, _urlDataStore, _timeProvider);
    }

    [Fact]
    public async Task Should_Return_Shortened_Url()
    {
        var request = CreateAddUrlRequest();

        var response = await _handler.HandleAsync(request, CancellationToken.None);

        response.Succeeded.Should().BeTrue();

        if (response.Succeeded)
        {
            response.Value.ShortUrl.Should().Be("1");
        }
    }

    [Fact]
    public async Task Should_Save_ShortUrl()
    {
        var request = CreateAddUrlRequest();

        var response = await _handler.HandleAsync(request, CancellationToken.None);

        response.Succeeded.Should().BeTrue();
        if (response.Succeeded)
        {
            _urlDataStore.Should().ContainKey(response.Value.ShortUrl);
        }
    }

    [Fact]
    public async Task Should_Save_ShortUrl_With_CreatedBy_And_CreatedOn()
    {
        var request = CreateAddUrlRequest();

        var response = await _handler.HandleAsync(request, CancellationToken.None);

        if (response.Succeeded)
        {
            response.Succeeded.Should().BeTrue();
            if (response.Succeeded)
            {
                _urlDataStore.Should().ContainKey(response.Value.ShortUrl);
                _urlDataStore[response.Value.ShortUrl].CreatedBy.Should().Be(request.CreatedBy);
                _urlDataStore[response.Value.ShortUrl].CreatedOn.Should().Be(_timeProvider.GetUtcNow());
            }
        }
    }

    [Fact]
    public async Task Should_Return_Error_If_CreatedBy_Is_Empty()
    {
        var request = CreateAddUrlRequest(string.Empty);

        var response = await _handler.HandleAsync(request, CancellationToken.None);

        response.Succeeded.Should().BeFalse();
        response.Error.Code.Should().Be("missing_value");
    }

    private static AddUrlRequest CreateAddUrlRequest(string createdBy = "vpuzdriak@vpuzdriak.com")
        => new(new Uri("https://google.com"), createdBy);
}