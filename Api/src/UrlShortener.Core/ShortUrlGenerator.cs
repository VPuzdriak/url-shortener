namespace UrlShortener.Core;

public sealed class ShortUrlGenerator(TokenProvider tokenProvider)
{
    public string GenerateUniqueUrl()
    {
        return tokenProvider.GetToken().EncodeToBase62();
    }
}