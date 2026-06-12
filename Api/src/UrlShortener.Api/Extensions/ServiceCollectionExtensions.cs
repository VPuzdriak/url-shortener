using UrlShortener.Core;
using UrlShortener.Core.Urls;
using UrlShortener.Core.Urls.Add;

namespace UrlShortener.Api.Extensions;

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddUrlFeature()
        {
            services.AddScoped<AddUrlHandler>();
            services.AddSingleton<TokenProvider>();
            services.AddScoped<ShortUrlGenerator>();

            return services;
        }
    }
}

internal sealed class InMemoryUrlDataStore : Dictionary<string, ShortenedUrl>, IUrlDataStore
{
    public Task AddAsync(ShortenedUrl shortened, CancellationToken cancellationToken)
    {
        Add(shortened.ShortUrl, shortened);
        return Task.CompletedTask;
    }
}