using Dapper;
using Master.Application.Ports;
using Master.Domain;
using Npgsql;

namespace Master.Infrastructure.Persistence;

public sealed class MasterItemRepository : IMasterItemRepository
{
    private readonly string _connectionString;

    public MasterItemRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<Guid> CreateAsync(MasterItem entity, CancellationToken cancellationToken = default)
    {
        const string sql = "insert into master.master_items(id, category, code, value, isactive, created_at_utc) values (@Id, @Category, @Code, @Value, @IsActive, @CreatedAtUtc);";
        await using var conn = new NpgsqlConnection(_connectionString);
        await conn.ExecuteAsync(new CommandDefinition(sql, entity, cancellationToken: cancellationToken));
        return entity.Id;
    }

    public async Task<MasterItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        const string sql = "select id, category as Category, code as Code, value as Value, isactive as IsActive, created_at_utc as CreatedAtUtc from master.master_items where id = @Id";
        await using var conn = new NpgsqlConnection(_connectionString);
        return await conn.QuerySingleOrDefaultAsync<MasterItem>(new CommandDefinition(sql, new { Id = id }, cancellationToken: cancellationToken));
    }

    public async Task<IReadOnlyCollection<MasterItem>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        const string sql = "select id, category as Category, code as Code, value as Value, isactive as IsActive, created_at_utc as CreatedAtUtc from master.master_items order by created_at_utc desc";
        await using var conn = new NpgsqlConnection(_connectionString);
        return (await conn.QueryAsync<MasterItem>(new CommandDefinition(sql, cancellationToken: cancellationToken))).ToArray();
    }

    public async Task<bool> UpdateAsync(MasterItem entity, CancellationToken cancellationToken = default)
    {
        const string sql = "update master.master_items set category = @Category,
                   code = @Code,
                   value = @Value,
                   isactive = @IsActive where id = @Id";
        await using var conn = new NpgsqlConnection(_connectionString);
        return await conn.ExecuteAsync(new CommandDefinition(sql, entity, cancellationToken: cancellationToken)) > 0;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        const string sql = "delete from master.master_items where id = @Id";
        await using var conn = new NpgsqlConnection(_connectionString);
        return await conn.ExecuteAsync(new CommandDefinition(sql, new { Id = id }, cancellationToken: cancellationToken)) > 0;
    }
}
