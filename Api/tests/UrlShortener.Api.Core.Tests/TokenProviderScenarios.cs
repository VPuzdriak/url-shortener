using System.Collections.Concurrent;
using FluentAssertions;
using UrlShortener.Core;

namespace UrlShortener.Api.Core.Tests;

public sealed class TokenProviderScenarios
{
    [Fact]
    public void Should_Get_The_Token_From_Start()
    {
        var provider = new TokenProvider();
        provider.AssignRange(5, 10);

        var token = provider.GetToken();

        token.Should().Be(5);
    }

    [Fact]
    public void Should_Increment_The_Token_On_Get()
    {
        var provider = new TokenProvider();
        provider.AssignRange(5, 10);

        var firstToken = provider.GetToken();
        var secondToken = provider.GetToken();

        firstToken.Should().Be(5);
        secondToken.Should().Be(6);
    }

    [Fact]
    public void Should_Not_Return_Same_Token_Twice()
    {
        var provider = new TokenProvider();
        ConcurrentBag<long> tokens = [];
        const int start = 1;
        const int end = 10_000;
        provider.AssignRange(start, end);

        Parallel.ForEach(Enumerable.Range(start, end),
            _ => tokens.Add(provider.GetToken()));

        tokens.Should().OnlyHaveUniqueItems();
    }
}