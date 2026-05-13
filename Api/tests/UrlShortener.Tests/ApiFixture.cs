using Microsoft.AspNetCore.Mvc.Testing;
using UrlShortener.Api;

namespace UrlShortener.Tests;

public sealed class ApiFixture : WebApplicationFactory<IApiAssemblyMarker>
{
}