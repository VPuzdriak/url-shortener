namespace UrlShortener.Core;

public static class Errors
{
    public static Error MissingCreatedBy => new("missing_value", "CreatedBy is required");
}