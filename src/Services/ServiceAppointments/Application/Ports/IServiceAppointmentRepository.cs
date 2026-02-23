using ServiceAppointments.Domain;

namespace ServiceAppointments.Application.Ports;

public interface IServiceAppointmentRepository
{
    Task<Guid> CreateAsync(ServiceAppointment entity, CancellationToken cancellationToken = default);
    Task<ServiceAppointment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<ServiceAppointment>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(ServiceAppointment entity, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
