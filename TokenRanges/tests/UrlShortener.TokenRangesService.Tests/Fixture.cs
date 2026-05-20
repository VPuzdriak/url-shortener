using DotNet.Testcontainers.Images;
using Microsoft.AspNetCore.Mvc.Testing;
using Npgsql;
using Testcontainers.PostgreSql;

namespace UrlShortener.TokenRangesService.Tests;

public sealed class Fixture : WebApplicationFactory<ITokenRangeAssemblyMarker>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgresContainer =
        new PostgreSqlBuilder(new DockerImage("postgres:18")).Build();

    private string ConnectionString => _postgresContainer.GetConnectionString();

    public async Task InitializeAsync()
    {
        await _postgresContainer.StartAsync();
        Environment.SetEnvironmentVariable("Postgres__ConnectionString", ConnectionString);

        await InitializeSqlTableAsync();
    }

    public new async Task DisposeAsync()
    {
        await _postgresContainer.StopAsync();
    }
    
    private async Task InitializeSqlTableAsync()
    {
        var tableSql = await File.ReadAllTextAsync("Table.sql");

        await using var connection = new NpgsqlConnection(ConnectionString);
        await connection.OpenAsync();

        await using var command = new NpgsqlCommand(tableSql, connection);
        await command.ExecuteNonQueryAsync();
    }
}