using Azure.Identity;
using UrlShortener.TokenRangesService;

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
builder.Services.AddSingleton(new TokenRangeManager(builder.Configuration["Postgres:ConnectionString"]!));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/", () => "TokenRanges Service");

app.MapPost("/assign", async (AssignTokenRangeRequest request, TokenRangeManager manager) =>
{
    var range = await manager.AssignRangeAsync(request.Key);
    return range;
});

app.Run();