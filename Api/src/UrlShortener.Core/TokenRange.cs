namespace UrlShortener.Core;

public record TokenRange
{
    public long Start { get; init; }
    public long End { get; init; }

    public TokenRange(long start, long end)
    {
        if (end < start)
        {
            throw new ArgumentException("End must be greater than or equal to Start");
        }

        Start = start;
        End = end;
    }
}