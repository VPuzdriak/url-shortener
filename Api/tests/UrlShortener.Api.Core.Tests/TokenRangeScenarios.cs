using UrlShortener.Code;

namespace UrlShortener.Api.Core.Tests;

public class TokenRangeScenarios
{
    [Fact]
    public void When_Start_Token_Is_Greater_Than_End_Token_Then_Throws_Exception()
    {
        var act = () => new TokenRange(100, 50);
        act.Should()
            .Throw<ArgumentException>().WithMessage("End must be greater than or equal to Start");
    }
}