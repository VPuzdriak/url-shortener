namespace UrlShortener.Api.Core.Tests;

public class TokenRangeScenarios
{
    [Fact]
    public void When_Start_Is_Greater_Than_End_Should_Throw_Exception()
    {
        var tokenProvider = new TokenProvider();
        var action = () => tokenProvider.AssignRange(10, 5);
        action.Should().Throw<ArgumentException>().WithMessage("End must be greater than or equal to Start");
    }
}