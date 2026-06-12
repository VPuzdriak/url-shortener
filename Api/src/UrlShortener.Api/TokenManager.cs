using UrlShortener.Api;
using UrlShortener.Core;

public sealed class TokenManager : IHostedService
{
    private readonly ITokenRangeApiClient _client;
    private readonly TokenProvider _tokenProvider;
    private readonly ILogger<TokenManager> _logger;
    private readonly IEnvironmentManager _environmentManager;
    private readonly string _machineIdentifier;

    public TokenManager(ITokenRangeApiClient client, TokenProvider tokenProvider, IEnvironmentManager environmentManager,  ILogger<TokenManager> logger)
    {
        _client = client;
        _tokenProvider = tokenProvider;
        _environmentManager = environmentManager;
        _logger = logger;
        _machineIdentifier = Environment.GetEnvironmentVariable("WEBSITE_INSTANCE_ID") ?? "unknown";
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Starting token manager");

            _tokenProvider.ReachingRangeLimit += async (_, _) => await AssignNewRange(cancellationToken);

            var range = await AssignNewRange(cancellationToken);
            _logger.LogInformation("Assigned range: {Start}-{End}", range.Start, range.End);
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "TokenManager failed to start due to an error");
            _environmentManager.FatalError();
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("TokenManager stopped");
        return Task.CompletedTask;
    }

    private async Task<TokenRange?> AssignNewRange(CancellationToken cancellationToken)
    {
        var range = await _client.AssignRangeAsync(_machineIdentifier, cancellationToken);

        if (range is null)
        {
            throw new Exception("No range assigned");
        }

        _tokenProvider.AssignRange(range);
        return range;
    }
}