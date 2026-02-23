using Master.Application.Dtos;
using Master.Application.Ports;
using Master.Domain;

namespace Master.Application.UseCases;

public sealed class MasterItemUseCase : IMasterItemUseCase
{
    private readonly IMasterItemRepository _repository;
    private readonly IMasterItemIntegrationPort _integration;

    public MasterItemUseCase(IMasterItemRepository repository, IMasterItemIntegrationPort integration)
    {
        _repository = repository;
        _integration = integration;
    }

    public async Task<Guid> CreateAsync(CreateMasterItemRequest request, CancellationToken cancellationToken = default)
    {
        var entity = new MasterItem
        {
            Id = Guid.NewGuid(),
            Category = request.Category,
            Code = request.Code,
            Value = request.Value,
            IsActive = request.IsActive,
            CreatedAtUtc = DateTime.UtcNow
        };

        var id = await _repository.CreateAsync(entity, cancellationToken);
        await _integration.PublishChangedAsync("master.created", new { id }, cancellationToken);
        return id;
    }

    public async Task<MasterItemDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => (await _repository.GetByIdAsync(id, cancellationToken)) is { } entity
            ? new MasterItemDto(entity.Id, entity.Category, entity.Code, entity.Value, entity.IsActive, entity.CreatedAtUtc)
            : null;

    public async Task<IReadOnlyCollection<MasterItemDto>> GetAllAsync(CancellationToken cancellationToken = default)
        => (await _repository.GetAllAsync(cancellationToken))
            .Select(entity => new MasterItemDto(entity.Id, entity.Category, entity.Code, entity.Value, entity.IsActive, entity.CreatedAtUtc))
            .ToArray();

    public async Task<bool> UpdateAsync(Guid id, UpdateMasterItemRequest request, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, cancellationToken);
        if (entity is null) return false;

        entity.Category = request.Category;
        entity.Code = request.Code;
        entity.Value = request.Value;
        entity.IsActive = request.IsActive;

        var updated = await _repository.UpdateAsync(entity, cancellationToken);
        if (updated)
            await _integration.PublishChangedAsync("master.updated", new { id = entity.Id }, cancellationToken);
        return updated;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var deleted = await _repository.DeleteAsync(id, cancellationToken);
        if (deleted)
            await _integration.PublishChangedAsync("master.deleted", new { id }, cancellationToken);
        return deleted;
    }
}
