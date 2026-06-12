using Microsoft.Extensions.Logging;
using NSubstitute;
using UrlShortener.Api;
using UrlShortener.Core;

namespace UrlShortener.Tests;

public sealed class TokenManagerScenarios
{
    [Fact]
    public async Task Should_Call_Api_On_Start()
    {
        var tokenRangeApiClient = Substitute.For<ITokenRangeApiClient>();
        tokenRangeApiClient.AssignRangeAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new TokenRange(1, 10));

        var tokenManager = new TokenManager(
            tokenRangeApiClient,
            Substitute.For<TokenProvider>(),
            Substitute.For<IEnvironmentManager>(),
            Substitute.For<ILogger<TokenManager>>()
        );

        await tokenManager.StartAsync(CancellationToken.None);

        await tokenRangeApiClient.Received().AssignRangeAsync(Arg.Any<string>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Should_Throw_Exception_When_No_Tokens_Assigned()
    {
        var tokenRangeApiClient = Substitute.For<ITokenRangeApiClient>();
        var envManager = Substitute.For<IEnvironmentManager>();
        
        var tokenManager = new TokenManager(
            tokenRangeApiClient,
            Substitute.For<TokenProvider>(),
            envManager,
            Substitute.For<ILogger<TokenManager>>()
        );

        await tokenManager.StartAsync(CancellationToken.None);
        envManager.Received().FatalError();
    }
}