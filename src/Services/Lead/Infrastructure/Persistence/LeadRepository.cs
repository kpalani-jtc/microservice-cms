using Dapper;
using Lead.Application.Ports;
using Lead.Domain;
using Npgsql;

namespace Lead.Infrastructure.Persistence;

public sealed class LeadRepository : ILeadRepository
{
    private readonly string _connectionString;

    public LeadRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<Guid> CreateAsync(Lead.Domain.Lead lead, CancellationToken cancellationToken = default)
    {
        const string sql = """
            insert into lead.leads(id, name, email, phone, status, created_at_utc)
            values (@Id, @Name, @Email, @Phone, @Status, @CreatedAtUtc);
            """;

        await using var conn = new NpgsqlConnection(_connectionString);
        await conn.ExecuteAsync(new CommandDefinition(sql, lead, cancellationToken: cancellationToken));
        return lead.Id;
    }

    public async Task<Lead.Domain.Lead?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        const string sql = "select id, name, email, phone, status, created_at_utc as CreatedAtUtc from lead.leads where id = @Id";
        await using var conn = new NpgsqlConnection(_connectionString);
        return await conn.QuerySingleOrDefaultAsync<Lead.Domain.Lead>(new CommandDefinition(sql, new { Id = id }, cancellationToken: cancellationToken));
    }

    public async Task<IReadOnlyCollection<Lead.Domain.Lead>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        const string sql = "select id, name, email, phone, status, created_at_utc as CreatedAtUtc from lead.leads order by created_at_utc desc";
        await using var conn = new NpgsqlConnection(_connectionString);
        return (await conn.QueryAsync<Lead.Domain.Lead>(new CommandDefinition(sql, cancellationToken: cancellationToken))).ToArray();
    }

    public async Task<bool> UpdateAsync(Lead.Domain.Lead lead, CancellationToken cancellationToken = default)
    {
        const string sql = """
            update lead.leads
               set name = @Name,
                   email = @Email,
                   phone = @Phone,
                   status = @Status
             where id = @Id;
            """;
        await using var conn = new NpgsqlConnection(_connectionString);
        return await conn.ExecuteAsync(new CommandDefinition(sql, lead, cancellationToken: cancellationToken)) > 0;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        const string sql = "delete from lead.leads where id = @Id";
        await using var conn = new NpgsqlConnection(_connectionString);
        return await conn.ExecuteAsync(new CommandDefinition(sql, new { Id = id }, cancellationToken: cancellationToken)) > 0;
    }
}
