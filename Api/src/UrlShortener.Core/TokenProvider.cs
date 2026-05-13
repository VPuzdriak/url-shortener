namespace UrlShortener.Core;

public sealed class TokenProvider
{
    private TokenRange? _tokenRange;

    public void AssignRange(long start, long end)
    {
        _tokenRange = new TokenRange(start, end);
    }
    
    public void AssignRange(TokenRange range)
    {
        _tokenRange = range;
    }

    public long GetToken()
    {
        return _tokenRange!.Start;
    }
}