namespace UrlShortener.Core.Urls.Add;

public sealed class ShortUrlGenerator(TokenProvider tokenProvider)
{
    public string GenerateUniqueUrl()
    {
        return tokenProvider.GetToken().EncodeToBase62();
    }
}