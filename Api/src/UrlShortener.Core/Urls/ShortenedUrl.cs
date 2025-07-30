namespace UrlShortener.Core.Urls;

public class ShortenedUrl
{
    public ShortenedUrl(Uri longUrl, string shortUrl, string createdBy, DateTimeOffset createdOn)
    {
        LongUrl = longUrl;
        ShortUrl = shortUrl;
        CreatedBy = createdBy;
        CreatedOn = createdOn;
    }


    public Uri LongUrl { get; }
    public string ShortUrl { get; }
    public string CreatedBy { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
}