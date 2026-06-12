using System.Collections.Concurrent;

namespace UrlShortener.Core;

public class TokenProvider
{
    private readonly Lock _tokenLock = new();
    private readonly ConcurrentQueue<TokenRange> _tokenRanges = new();

    private long _currentToken;
    private TokenRange? _currentTokenRange;

    public void AssignRange(long start, long end)
    {
        AssignRange(new TokenRange(start, end));
    }

    public void AssignRange(TokenRange tokenRange)
    {
        _tokenRanges.Enqueue(tokenRange);
    }

    public long GetToken()
    {
        lock (_tokenLock)
        {
            if (_currentTokenRange is null)
            {
                MoveToNextRange();
            }

            if (_currentToken > _currentTokenRange?.End)
            {
                MoveToNextRange();
            }

            if (IsReachingRangeLimit())
            {
                OnReachingRangeLimit(new ReachingRangeLimitEventArgs
                {
                    RangeLimit = _currentTokenRange!.End,
                    Token = _currentToken
                });
            }

            return _currentToken++;
        }
    }

    public event EventHandler? ReachingRangeLimit;

    protected void OnReachingRangeLimit(EventArgs e)
    {
        ReachingRangeLimit?.Invoke(this, e);
    }

    private bool IsReachingRangeLimit()
    {
        var total = _currentTokenRange!.End - _currentTokenRange.Start;
        var currentIndex = _currentToken + 1 - _currentTokenRange.Start;
        return currentIndex >= total * 0.8;
    }

    private void MoveToNextRange()
    {
        if (!_tokenRanges.TryDequeue(out _currentTokenRange))
        {
            throw new IndexOutOfRangeException("No more token ranges available.");
        }

        _currentToken = _currentTokenRange.Start;
    }
}

public class ReachingRangeLimitEventArgs : EventArgs
{
    public long Token { get; set; }
    public long RangeLimit { get; set; }
}