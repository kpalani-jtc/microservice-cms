using Dapper;
using Complaints.Application.Ports;
using Complaints.Domain;
using Npgsql;

namespace Complaints.Infrastructure.Persistence;

public sealed class ComplaintRepository : IComplaintRepository
{
    private readonly string _connectionString;

    public ComplaintRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<Guid> CreateAsync(Complaint entity, CancellationToken cancellationToken = default)
    {
        const string sql = "insert into complaints.complaints(id, title, description, status, created_at_utc) values (@Id, @Title, @Description, @Status, @CreatedAtUtc);";
        await using var conn = new NpgsqlConnection(_connectionString);
        await conn.ExecuteAsync(new CommandDefinition(sql, entity, cancellationToken: cancellationToken));
        return entity.Id;
    }

    public async Task<Complaint?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        const string sql = "select id, title as Title, description as Description, status as Status, created_at_utc as CreatedAtUtc from complaints.complaints where id = @Id";
        await using var conn = new NpgsqlConnection(_connectionString);
        return await conn.QuerySingleOrDefaultAsync<Complaint>(new CommandDefinition(sql, new { Id = id }, cancellationToken: cancellationToken));
    }

    public async Task<IReadOnlyCollection<Complaint>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        const string sql = "select id, title as Title, description as Description, status as Status, created_at_utc as CreatedAtUtc from complaints.complaints order by created_at_utc desc";
        await using var conn = new NpgsqlConnection(_connectionString);
        return (await conn.QueryAsync<Complaint>(new CommandDefinition(sql, cancellationToken: cancellationToken))).ToArray();
    }

    public async Task<bool> UpdateAsync(Complaint entity, CancellationToken cancellationToken = default)
    {
        const string sql = "update complaints.complaints set title = @Title,
                   description = @Description,
                   status = @Status where id = @Id";
        await using var conn = new NpgsqlConnection(_connectionString);
        return await conn.ExecuteAsync(new CommandDefinition(sql, entity, cancellationToken: cancellationToken)) > 0;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        const string sql = "delete from complaints.complaints where id = @Id";
        await using var conn = new NpgsqlConnection(_connectionString);
        return await conn.ExecuteAsync(new CommandDefinition(sql, new { Id = id }, cancellationToken: cancellationToken)) > 0;
    }
}
