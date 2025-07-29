namespace UrlShortener.Code;

public class ShortUrlGenerator
{
    private readonly TokenProvider _tokenProvider;

    public ShortUrlGenerator(TokenProvider tokenProvider)
    {
        _tokenProvider = tokenProvider;
    }

    public string GenerateUniqueUrl() => _tokenProvider.GetToken().EncodeToBase62();
}