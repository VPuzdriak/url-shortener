namespace UrlShortener.Core;

public sealed class TokenProvider
{
    private readonly Lock _tokenLock = new();

    private long _token = 0;
    private TokenRange? _tokenRange;

    public void AssignRange(long start, long end)
    {
        AssignRange(new TokenRange(start, end));
    }

    public void AssignRange(TokenRange range)
    {
        _tokenRange = range;

        lock (_tokenLock)
        {
            _token = _tokenRange.Start;
        }
    }

    public long GetToken()
    {
        lock (_tokenLock)
        {
            return _token++;
        }
    }
}