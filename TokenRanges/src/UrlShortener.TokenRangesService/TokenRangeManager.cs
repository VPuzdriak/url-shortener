using Npgsql;

namespace UrlShortener.TokenRangesService;

internal sealed class TokenRangeManager(string connectionString)
{
    private const int DefaultRangeSize = 1000;

    private readonly string _sqlQuery =
        $$"""
           INSERT INTO "TokenRanges" ("MachineIdentifier", "Start", "End")
           VALUES (
               @MachineIdentifier,
               COALESCE((SELECT MAX("End") FROM "TokenRanges") + 1, {{DefaultRangeSize}}),
               COALESCE((SELECT MAX("End") FROM "TokenRanges") + {{DefaultRangeSize}}, 2000)
           )
           RETURNING "Id", "MachineIdentifier", "Start", "End";
          """;


    public async Task<TokenRangeResponse> AssignRangeAsync(string machineIdentifier)
    {
        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();

        await using var transaction = await connection.BeginTransactionAsync();

        await using var lockCmd = new NpgsqlCommand("SELECT pg_advisory_xact_lock(1)", connection, transaction);
        await lockCmd.ExecuteNonQueryAsync();
        
        await using var command = new NpgsqlCommand(_sqlQuery, connection, transaction);
        command.Parameters.AddWithValue("@MachineIdentifier", machineIdentifier);

        TokenRangeResponse? response = null;
        await using (var reader = await command.ExecuteReaderAsync())
        {
            if (await reader.ReadAsync())
            {
                var start = reader.GetInt64(2);
                var end = reader.GetInt64(3);
                response = new TokenRangeResponse(start, end);
            }
        }

        if (response is null)
        {
            throw new FailedToAssignRangeException("Failed to assign range.");
        }

        await transaction.CommitAsync();
        return response;
    }
}