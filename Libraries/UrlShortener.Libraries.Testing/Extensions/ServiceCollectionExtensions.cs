using Microsoft.Extensions.DependencyInjection;

namespace UrlShortener.Libraries.Testing.Extensions;

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public void Remove<T>()
        {
            var descriptor = services.Single(d => d.ServiceType == typeof(T));
            services.Remove(descriptor);
        }
    }
}