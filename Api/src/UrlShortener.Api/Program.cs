using Azure.Identity;
using UrlShortener.Api.Extensions;
using UrlShortener.Core.Urls.Add;

var builder = WebApplication.CreateBuilder(args);

var keyVaultName = builder.Configuration["KeyVaultName"];

if (!string.IsNullOrEmpty(keyVaultName))
{
    builder.Configuration.AddAzureKeyVault(
        new Uri($"https://{keyVaultName}.vault.azure.net/"),
        new DefaultAzureCredential());
}

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddUrlFeature();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/api/urls", async (AddUrlHandler handler, AddUrlRequest request, CancellationToken cancellationToken) =>
{
    var requestWithUser = request with
    {
        CreatedBy = "volodymyr@volodymyrpuzdriak.com"
    };

    var result = await handler.HandleAsync(requestWithUser, cancellationToken);

    return result.Succeeded
        ? Results.Created($"/api/urls/{result.Value.ShortUrl}", result.Value)
        : Results.BadRequest(result.Error);
});

app.Run();