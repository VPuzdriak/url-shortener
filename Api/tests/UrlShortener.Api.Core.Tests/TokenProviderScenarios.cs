using System.Collections.Concurrent;
using UrlShortener.Core;

namespace UrlShortener.Api.Core.Tests;

public class TokenProviderScenarios
{
    [Fact]
    public void Should_Get_The_Token_From_Start()
    {
        var tokenProvider = new TokenProvider();

        tokenProvider.AssignRange(100, 200);

        tokenProvider.GetToken().Should().Be(100);
    }

    [Fact]
    public void Should_Increment_The_Token_On_Get()
    {
        var tokenProvider = new TokenProvider();

        tokenProvider.AssignRange(100, 200);
        tokenProvider.GetToken();

        tokenProvider.GetToken().Should().Be(101);
    }

    [Fact]
    public void Should_Not_Return_Same_Token_Twice()
    {
        var tokenProvider = new TokenProvider();
        const int start = 1;
        const int end = 1000;
        tokenProvider.AssignRange(100, 200);

        var tokens = new ConcurrentBag<long>();
        Parallel.ForEach(Enumerable.Range(start, end), _ => tokens.Add(tokenProvider.GetToken()));

        tokens.Should().OnlyHaveUniqueItems();
    }
}