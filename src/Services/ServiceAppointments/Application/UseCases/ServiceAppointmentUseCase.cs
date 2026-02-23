using ServiceAppointments.Application.Dtos;
using ServiceAppointments.Application.Ports;
using ServiceAppointments.Domain;

namespace ServiceAppointments.Application.UseCases;

public sealed class ServiceAppointmentUseCase : IServiceAppointmentUseCase
{
    private readonly IServiceAppointmentRepository _repository;
    private readonly IServiceAppointmentIntegrationPort _integration;

    public ServiceAppointmentUseCase(IServiceAppointmentRepository repository, IServiceAppointmentIntegrationPort integration)
    {
        _repository = repository;
        _integration = integration;
    }

    public async Task<Guid> CreateAsync(CreateServiceAppointmentRequest request, CancellationToken cancellationToken = default)
    {
        var entity = new ServiceAppointment
        {
            Id = Guid.NewGuid(),
            LeadId = request.LeadId,
            ScheduledAtUtc = request.ScheduledAtUtc,
            Technician = request.Technician,
            Status = request.Status,
            CreatedAtUtc = DateTime.UtcNow
        };

        var id = await _repository.CreateAsync(entity, cancellationToken);
        await _integration.PublishChangedAsync("appointment.created", new { id }, cancellationToken);
        return id;
    }

    public async Task<ServiceAppointmentDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => (await _repository.GetByIdAsync(id, cancellationToken)) is { } entity
            ? new ServiceAppointmentDto(entity.Id, entity.LeadId, entity.ScheduledAtUtc, entity.Technician, entity.Status, entity.CreatedAtUtc)
            : null;

    public async Task<IReadOnlyCollection<ServiceAppointmentDto>> GetAllAsync(CancellationToken cancellationToken = default)
        => (await _repository.GetAllAsync(cancellationToken))
            .Select(entity => new ServiceAppointmentDto(entity.Id, entity.LeadId, entity.ScheduledAtUtc, entity.Technician, entity.Status, entity.CreatedAtUtc))
            .ToArray();

    public async Task<bool> UpdateAsync(Guid id, UpdateServiceAppointmentRequest request, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, cancellationToken);
        if (entity is null) return false;

        entity.LeadId = request.LeadId;
        entity.ScheduledAtUtc = request.ScheduledAtUtc;
        entity.Technician = request.Technician;
        entity.Status = request.Status;

        var updated = await _repository.UpdateAsync(entity, cancellationToken);
        if (updated)
            await _integration.PublishChangedAsync("appointment.updated", new { id = entity.Id }, cancellationToken);
        return updated;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var deleted = await _repository.DeleteAsync(id, cancellationToken);
        if (deleted)
            await _integration.PublishChangedAsync("appointment.deleted", new { id }, cancellationToken);
        return deleted;
    }
}
