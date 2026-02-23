using Master.Application.Dtos;

namespace Master.Application.Ports;

public interface IMasterItemUseCase
{
    Task<Guid> CreateAsync(CreateMasterItemRequest request, CancellationToken cancellationToken = default);
    Task<MasterItemDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<MasterItemDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(Guid id, UpdateMasterItemRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
