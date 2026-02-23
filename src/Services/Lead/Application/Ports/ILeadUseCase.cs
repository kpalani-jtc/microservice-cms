using Lead.Application.Dtos;

namespace Lead.Application.Ports;

public interface ILeadUseCase
{
    Task<Guid> CreateAsync(CreateLeadRequest request, CancellationToken cancellationToken = default);
    Task<LeadDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<LeadDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(Guid id, UpdateLeadRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
