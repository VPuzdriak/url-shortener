using Microsoft.Azure.Cosmos;

namespace UrlShortener.RedirectApi.Infrastructure;

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddUrlReader(
            string cosmosDbConnectionString,
            string databaseName,
            string containerName)
        {
            services.AddSingleton<CosmosClient>(s => new CosmosClient(connectionString: cosmosDbConnectionString));
            services.AddSingleton<IShortenedUrlReader>(s =>
            {
                var cosmosClient = s.GetRequiredService<CosmosClient>();
                var container = cosmosClient.GetContainer(databaseName, containerName);
                return new CosmosShortenedUrlReader(container);
            });

            return services;
        }
    }
}