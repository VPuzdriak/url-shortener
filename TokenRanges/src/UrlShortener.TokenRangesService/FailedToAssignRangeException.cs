namespace UrlShortener.TokenRangesService;

public sealed class FailedToAssignRangeException(string message) : Exception(message);