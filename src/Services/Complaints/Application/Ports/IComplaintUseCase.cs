using Complaints.Application.Dtos;

namespace Complaints.Application.Ports;

public interface IComplaintUseCase
{
    Task<Guid> CreateAsync(CreateComplaintRequest request, CancellationToken cancellationToken = default);
    Task<ComplaintDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<ComplaintDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(Guid id, UpdateComplaintRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
