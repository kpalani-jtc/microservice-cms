using Complaints.Application.Dtos;
using Complaints.Application.Ports;
using Complaints.Domain;

namespace Complaints.Application.UseCases;

public sealed class ComplaintUseCase : IComplaintUseCase
{
    private readonly IComplaintRepository _repository;
    private readonly IComplaintIntegrationPort _integration;

    public ComplaintUseCase(IComplaintRepository repository, IComplaintIntegrationPort integration)
    {
        _repository = repository;
        _integration = integration;
    }

    public async Task<Guid> CreateAsync(CreateComplaintRequest request, CancellationToken cancellationToken = default)
    {
        var entity = new Complaint
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            Status = request.Status,
            CreatedAtUtc = DateTime.UtcNow
        };

        var id = await _repository.CreateAsync(entity, cancellationToken);
        await _integration.PublishChangedAsync("complaint.created", new { id }, cancellationToken);
        return id;
    }

    public async Task<ComplaintDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => (await _repository.GetByIdAsync(id, cancellationToken)) is { } entity
            ? new ComplaintDto(entity.Id, entity.Title, entity.Description, entity.Status, entity.CreatedAtUtc)
            : null;

    public async Task<IReadOnlyCollection<ComplaintDto>> GetAllAsync(CancellationToken cancellationToken = default)
        => (await _repository.GetAllAsync(cancellationToken))
            .Select(entity => new ComplaintDto(entity.Id, entity.Title, entity.Description, entity.Status, entity.CreatedAtUtc))
            .ToArray();

    public async Task<bool> UpdateAsync(Guid id, UpdateComplaintRequest request, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, cancellationToken);
        if (entity is null) return false;

        entity.Title = request.Title;
        entity.Description = request.Description;
        entity.Status = request.Status;

        var updated = await _repository.UpdateAsync(entity, cancellationToken);
        if (updated)
            await _integration.PublishChangedAsync("complaint.updated", new { id = entity.Id }, cancellationToken);
        return updated;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var deleted = await _repository.DeleteAsync(id, cancellationToken);
        if (deleted)
            await _integration.PublishChangedAsync("complaint.deleted", new { id }, cancellationToken);
        return deleted;
    }
}
