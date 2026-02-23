using Master.Domain;

namespace Master.Application.Ports;

public interface IMasterItemRepository
{
    Task<Guid> CreateAsync(MasterItem entity, CancellationToken cancellationToken = default);
    Task<MasterItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<MasterItem>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(MasterItem entity, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
