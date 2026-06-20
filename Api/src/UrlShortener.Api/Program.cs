using System.Security.Authentication;
using System.Security.Claims;
using Azure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;
using UrlShortener.Api;
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
builder.Services.AddSingleton<IEnvironmentManager, EnvironmentManager>();

builder.Services.AddHttpClient("TokenRangeService",
    client => client.BaseAddress = new Uri(builder.Configuration["TokenRangeService:Endpoint"]!)
);
builder.Services.AddSingleton<ITokenRangeApiClient, TokenRangeApiClient>();
builder.Services.AddHostedService<TokenManager>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(jwtBearerOptions =>
        {
            builder.Configuration.Bind("AzureAd", jwtBearerOptions);
            jwtBearerOptions.TokenValidationParameters.NameClaimType = "name";
        },
        identityOptions => builder.Configuration.Bind("AzureAd", identityOptions));

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("AuthZPolicy", policyBuilder =>
    {
        policyBuilder.Requirements.Add(new ScopeAuthorizationRequirement
        {
            RequiredScopesConfigurationKey = "AzureAd:Scopes",
        });
    });

builder.Services.AddAuthorization(options =>
{
    options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
        .RequireAuthenticatedUser()
        .Build();

    options.FallbackPolicy = options.DefaultPolicy;
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () => "API")
    .AllowAnonymous();

app.MapPost("/api/urls", async (
    AddUrlHandler handler,
    AddUrlRequest request,
    HttpContext context,
    CancellationToken cancellationToken) =>
{
    var email = context.User.FindFirstValue("preferred_username")
                ?? throw new AuthenticationException("Missing preferred_username claim");

    var requestWithUser = request with { CreatedBy = email };
    var result = await handler.HandleAsync(requestWithUser, cancellationToken);
    return result.Match(
        response => Results.Created($"/api/urls/{response.ShortUrl}", response),
        Results.BadRequest
    );
});

app.Run();