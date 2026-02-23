using Dapper;
using ServiceAppointments.Application.Ports;
using ServiceAppointments.Domain;
using Npgsql;

namespace ServiceAppointments.Infrastructure.Persistence;

public sealed class ServiceAppointmentRepository : IServiceAppointmentRepository
{
    private readonly string _connectionString;

    public ServiceAppointmentRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<Guid> CreateAsync(ServiceAppointment entity, CancellationToken cancellationToken = default)
    {
        const string sql = "insert into appointments.service_appointments(id, leadid, scheduledatutc, technician, status, created_at_utc) values (@Id, @LeadId, @ScheduledAtUtc, @Technician, @Status, @CreatedAtUtc);";
        await using var conn = new NpgsqlConnection(_connectionString);
        await conn.ExecuteAsync(new CommandDefinition(sql, entity, cancellationToken: cancellationToken));
        return entity.Id;
    }

    public async Task<ServiceAppointment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        const string sql = "select id, leadid as LeadId, scheduledatutc as ScheduledAtUtc, technician as Technician, status as Status, created_at_utc as CreatedAtUtc from appointments.service_appointments where id = @Id";
        await using var conn = new NpgsqlConnection(_connectionString);
        return await conn.QuerySingleOrDefaultAsync<ServiceAppointment>(new CommandDefinition(sql, new { Id = id }, cancellationToken: cancellationToken));
    }

    public async Task<IReadOnlyCollection<ServiceAppointment>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        const string sql = "select id, leadid as LeadId, scheduledatutc as ScheduledAtUtc, technician as Technician, status as Status, created_at_utc as CreatedAtUtc from appointments.service_appointments order by created_at_utc desc";
        await using var conn = new NpgsqlConnection(_connectionString);
        return (await conn.QueryAsync<ServiceAppointment>(new CommandDefinition(sql, cancellationToken: cancellationToken))).ToArray();
    }

    public async Task<bool> UpdateAsync(ServiceAppointment entity, CancellationToken cancellationToken = default)
    {
        const string sql = "update appointments.service_appointments set leadid = @LeadId,
                   scheduledatutc = @ScheduledAtUtc,
                   technician = @Technician,
                   status = @Status where id = @Id";
        await using var conn = new NpgsqlConnection(_connectionString);
        return await conn.ExecuteAsync(new CommandDefinition(sql, entity, cancellationToken: cancellationToken)) > 0;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        const string sql = "delete from appointments.service_appointments where id = @Id";
        await using var conn = new NpgsqlConnection(_connectionString);
        return await conn.ExecuteAsync(new CommandDefinition(sql, new { Id = id }, cancellationToken: cancellationToken)) > 0;
    }
}
