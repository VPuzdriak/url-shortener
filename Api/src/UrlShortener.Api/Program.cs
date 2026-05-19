using Azure.Identity;
using UrlShortener.Api.Extensions;
using UrlShortener.Core.Urls.Add;
using UrlShortener.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

var keyVaultName = builder.Configuration["KeyVaultName"];
if (!string.IsNullOrEmpty(keyVaultName))
{
    builder.Configuration.AddAzureKeyVault(
        new Uri($"https://{keyVaultName}.vault.azure.net/"),
        new DefaultAzureCredential()
    );
}

builder.Services.AddOpenApi();
builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddUrlFeature();
builder.Services.AddCosmosUrlDataStore(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/", () => "API");

app.MapPost("/api/urls", async (
    AddUrlHandler handler,
    AddUrlRequest request,
    CancellationToken cancellationToken) =>
{
    var requestWithUser = request with { CreatedBy = "vpuzdriak@vpuzdriak.com" };
    var result = await handler.HandleAsync(requestWithUser, cancellationToken);
    return result.Match(
        response => Results.Created($"/api/urls/{response.ShortUrl}", response),
        Results.BadRequest
    );
});

app.Run();