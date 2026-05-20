using System.Collections.Concurrent;
using System.Net.Http.Json;
using FluentAssertions;

namespace UrlShortener.TokenRangesService.Tests;

public sealed class AssignTokenRangeScenarios(Fixture fixture) : IClassFixture<Fixture>
{
    private readonly HttpClient _client = fixture.CreateClient();

    [Fact]
    public async Task Should_Return_Range_When_Requested()
    {
        var response = await _client.PostAsJsonAsync("/assign", new AssignTokenRangeRequest("tests"));
        response.EnsureSuccessStatusCode();

        var tokenRange = await response.Content.ReadFromJsonAsync<TokenRangeResponse>();

        tokenRange!.Start.Should().BeGreaterThan(0);
        tokenRange.End.Should().BeGreaterThan(tokenRange.Start);
    }

    [Fact]
    public async Task Should_Not_Repeat_Range_When_Requested()
    {
        var response1 = await _client.PostAsJsonAsync("/assign", new AssignTokenRangeRequest("tests"));
        response1.EnsureSuccessStatusCode();
        var response2 = await _client.PostAsJsonAsync("/assign", new AssignTokenRangeRequest("tests"));
        response2.EnsureSuccessStatusCode();

        var tokenRange1 = await response1.Content.ReadFromJsonAsync<TokenRangeResponse>();
        var tokenRange2 = await response2.Content.ReadFromJsonAsync<TokenRangeResponse>();

        tokenRange2!.Start.Should().BeGreaterThan(tokenRange1!.End);
    }

    [Fact]
    public async Task Should_Not_Repeat_Range_On_Multiple_Requests()
    {
        ConcurrentBag<TokenRangeResponse> ranges = [];

        await Parallel.ForEachAsync(Enumerable.Range(1, 100), async (number, cancellationToken) =>
        {
            var response = await _client
                .PostAsJsonAsync("/assign", new AssignTokenRangeRequest(number.ToString()), cancellationToken);
            response.EnsureSuccessStatusCode();

            var range = await response.Content.ReadFromJsonAsync<TokenRangeResponse>(cancellationToken);
            ranges.Add(range!);
        });

        ranges.Should().OnlyHaveUniqueItems(r => r.Start);
        ranges.Should().OnlyHaveUniqueItems(r => r.End);
    }
}