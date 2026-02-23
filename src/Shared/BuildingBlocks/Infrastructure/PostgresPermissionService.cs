using BuildingBlocks.Application;
using Dapper;
using Npgsql;

namespace BuildingBlocks.Infrastructure;

public sealed class PostgresPermissionService : IPermissionService
{
    private readonly string _connectionString;

    public PostgresPermissionService(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<bool> HasPermissionAsync(string subjectId, string permission, CancellationToken cancellationToken = default)
    {
        const string sql = @"
select exists (
    select 1
    from auth.users u
    join auth.user_roles ur on ur.user_id = u.id and (ur.expires_at is null or ur.expires_at > now())
    join auth.role_permissions rp on rp.role_id = ur.role_id
    join auth.permissions p on p.id = rp.permission_id
    where u.external_subject = @Subject and p.code = @Permission
);";

        await using var conn = new NpgsqlConnection(_connectionString);
        return await conn.ExecuteScalarAsync<bool>(new CommandDefinition(sql, new { Subject = subjectId, Permission = permission }, cancellationToken: cancellationToken));
    }
}
