using FluentAssertions;
using UrlShortener.Core;

namespace UrlShortener.Api.Core.Tests;

public sealed class TokenRangeScenarios
{
    [Fact]
    public void When_Start_Token_Is_GT_End_Token_Then_Throws_Exception()
    {
        var act = () => new TokenRange(10, 5);

        act.Should()
            .Throw<ArgumentException>()
            .WithMessage("End must be greater than or equal to start");
    }
}