namespace UrlShortener.Core.Urls.Add;

public sealed class AddUrlHandler(
    ShortUrlGenerator shortUrlGenerator,
    IUrlDataStore urlDataStore,
    TimeProvider timeProvider)
{
    public async Task<Result<AddUrlResponse>> HandleAsync(AddUrlRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.CreatedBy))
        {
            return Errors.MissingCreatedBy;
        }

        var shortUrl = shortUrlGenerator.GenerateUniqueUrl();
        var shortened = new ShortenedUrl(request.LongUrl, shortUrl, request.CreatedBy, timeProvider.GetUtcNow());

        await urlDataStore.AddAsync(shortened, cancellationToken);

        return new AddUrlResponse(request.LongUrl, shortUrl);
    }
}