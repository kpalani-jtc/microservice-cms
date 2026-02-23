using ServiceAppointments.Application.Dtos;

namespace ServiceAppointments.Application.Ports;

public interface IServiceAppointmentUseCase
{
    Task<Guid> CreateAsync(CreateServiceAppointmentRequest request, CancellationToken cancellationToken = default);
    Task<ServiceAppointmentDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<ServiceAppointmentDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(Guid id, UpdateServiceAppointmentRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
