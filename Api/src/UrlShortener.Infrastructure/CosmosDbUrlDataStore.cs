using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;
using UrlShortener.Core.Urls;
using UrlShortener.Core.Urls.Add;

namespace UrlShortener.Infrastructure;

public sealed class CosmosDbUrlDataStore(Container container) : IUrlDataStore
{
    public async Task AddAsync(ShortenedUrl shortened, CancellationToken cancellationToken)
    {
        var document = (ShortenedUrlCosmos)shortened;
        await container.CreateItemAsync(
            document,
            new PartitionKey(document.PartitionKey),
            cancellationToken: cancellationToken);
    }
}

public sealed class ShortenedUrlCosmos
{
    public string LongUrl { get; }
    [JsonProperty(PropertyName = "id")] public string ShortUrl { get; }
    public DateTimeOffset CreatedOn { get; }
    public string CreatedBy { get; }

    public string PartitionKey => ShortUrl[..1];

    public ShortenedUrlCosmos(string longUrl, string shortUrl, DateTimeOffset createdOn, string createdBy)
    {
        LongUrl = longUrl;
        ShortUrl = shortUrl;
        CreatedOn = createdOn;
        CreatedBy = createdBy;
    }
    
    public static implicit operator ShortenedUrl(ShortenedUrlCosmos url) =>
        new(new Uri(url.LongUrl), url.ShortUrl, url.CreatedBy, url.CreatedOn);
    
    public static explicit operator ShortenedUrlCosmos(ShortenedUrl shortened) =>
        new(shortened.LongUrl.ToString(), shortened.ShortUrl, shortened.CreatedOn, shortened.CreatedBy);
}