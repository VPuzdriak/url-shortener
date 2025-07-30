using System.Text.Json.Serialization;
using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;
using UrlShortener.Core.Urls;

namespace UrlShortener.Infrastructure;

public class CosmosDbUrlDataStore : IUrlDataStore
{
    private readonly Container _container;

    public CosmosDbUrlDataStore(Container container)
    {
        _container = container;
    }

    public async Task AddAsync(ShortenedUrl shortened, CancellationToken cancellationToken)
    {
        var document = (ShortenedUrlCosmos)shortened;
        await _container.CreateItemAsync(document, new PartitionKey(document.PartitionKey),
            cancellationToken: cancellationToken);
    }
}

internal class ShortenedUrlCosmos
{
    public string LongUrl { get; }

    [JsonProperty(PropertyName = "id")] // Cosmos DB unique identifier
    public string ShortUrl { get; }

    public DateTimeOffset CreatedOn { get; }
    public string CreatedBy { get; }

    public string PartitionKey => ShortUrl[..1]; // Cosmos DB partition key

    public ShortenedUrlCosmos(string longUrl, string shortUrl, DateTimeOffset createdOn, string createdBy)
    {
        LongUrl = longUrl;
        ShortUrl = shortUrl;
        CreatedOn = createdOn;
        CreatedBy = createdBy;
    }

    public static implicit operator ShortenedUrl(ShortenedUrlCosmos cosmos) =>
        new(new Uri(cosmos.LongUrl), cosmos.ShortUrl, cosmos.CreatedBy, cosmos.CreatedOn);

    public static explicit operator ShortenedUrlCosmos(ShortenedUrl shortened) =>
        new(shortened.LongUrl.ToString(), shortened.ShortUrl, shortened.CreatedOn, shortened.CreatedBy);
}