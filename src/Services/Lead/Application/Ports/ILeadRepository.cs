using Lead.Domain;

namespace Lead.Application.Ports;

public interface ILeadRepository
{
    Task<Guid> CreateAsync(Lead lead, CancellationToken cancellationToken = default);
    Task<Lead?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Lead>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(Lead lead, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
