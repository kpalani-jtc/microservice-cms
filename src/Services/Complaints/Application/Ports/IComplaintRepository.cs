using Complaints.Domain;

namespace Complaints.Application.Ports;

public interface IComplaintRepository
{
    Task<Guid> CreateAsync(Complaint entity, CancellationToken cancellationToken = default);
    Task<Complaint?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Complaint>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(Complaint entity, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
