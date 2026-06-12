using System.Collections.Concurrent;
using FluentAssertions;
using UrlShortener.Core;

namespace UrlShortener.Api.Core.Tests;

public sealed class TokenProviderScenarios
{
    private TokenProvider _provider = new();

    [Fact]
    public void Should_Get_The_Token_From_Start()
    {
        _provider.AssignRange(5, 10);

        var token = _provider.GetToken();

        token.Should().Be(5);
    }

    [Fact]
    public void Should_Increment_The_Token_On_Get()
    {
        _provider.AssignRange(5, 10);

        var firstToken = _provider.GetToken();
        var secondToken = _provider.GetToken();

        firstToken.Should().Be(5);
        secondToken.Should().Be(6);
    }

    [Fact]
    public void Should_Not_Return_Same_Token_Twice()
    {
        ConcurrentBag<long> tokens = [];
        const int start = 1;
        const int end = 10_000;
        _provider.AssignRange(start, end);

        Parallel.ForEach(Enumerable.Range(start, end),
            _ => tokens.Add(_provider.GetToken()));

        tokens.Should().OnlyHaveUniqueItems();
    }

    [Fact]
    public void Should_Use_Multiple_Ranges()
    {
        _provider = new TokenProvider();
        _provider.AssignRange(1, 2);
        _provider.AssignRange(42, 45);
        _provider.GetToken(); // 1
        _provider.GetToken(); // 2

        // First token range is exhausted, should move to the next one
        var token = _provider.GetToken();

        token.Should().Be(42);
    }

    [Fact]
    public void Should_Trigger_ReachingRangeLimit_Event_When_Range_Is_At_80_Percent()
    {
        _provider.AssignRange(1, 10);
        bool eventTriggered = false;
        _provider.ReachingRangeLimit += (_, _) => eventTriggered = true;

        for (int i = 0; i < 8; i++)
        {
            _provider.GetToken();
        }

        eventTriggered.Should().BeTrue();
    }
}