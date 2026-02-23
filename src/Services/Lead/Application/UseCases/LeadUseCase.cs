using Lead.Application.Dtos;
using Lead.Application.Ports;
using Lead.Domain;

namespace Lead.Application.UseCases;

public sealed class LeadUseCase : ILeadUseCase
{
    private readonly ILeadRepository _repository;
    private readonly ILeadIntegrationPort _integration;

    public LeadUseCase(ILeadRepository repository, ILeadIntegrationPort integration)
    {
        _repository = repository;
        _integration = integration;
    }

    public async Task<Guid> CreateAsync(CreateLeadRequest request, CancellationToken cancellationToken = default)
    {
        var lead = new Lead
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Email = request.Email,
            Phone = request.Phone,
            Status = "New",
            CreatedAtUtc = DateTime.UtcNow
        };

        var id = await _repository.CreateAsync(lead, cancellationToken);
        await _integration.PublishLeadChangedAsync("lead.created", new { id, lead.Name, lead.Email }, cancellationToken);
        return id;
    }

    public async Task<LeadDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => (await _repository.GetByIdAsync(id, cancellationToken)) is { } lead
            ? new LeadDto(lead.Id, lead.Name, lead.Email, lead.Phone, lead.Status, lead.CreatedAtUtc)
            : null;

    public async Task<IReadOnlyCollection<LeadDto>> GetAllAsync(CancellationToken cancellationToken = default)
        => (await _repository.GetAllAsync(cancellationToken))
            .Select(l => new LeadDto(l.Id, l.Name, l.Email, l.Phone, l.Status, l.CreatedAtUtc))
            .ToArray();

    public async Task<bool> UpdateAsync(Guid id, UpdateLeadRequest request, CancellationToken cancellationToken = default)
    {
        var lead = await _repository.GetByIdAsync(id, cancellationToken);
        if (lead is null) return false;

        lead.Name = request.Name;
        lead.Email = request.Email;
        lead.Phone = request.Phone;
        lead.Status = request.Status;

        var updated = await _repository.UpdateAsync(lead, cancellationToken);
        if (updated)
            await _integration.PublishLeadChangedAsync("lead.updated", new { id = lead.Id, lead.Status }, cancellationToken);

        return updated;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var deleted = await _repository.DeleteAsync(id, cancellationToken);
        if (deleted)
            await _integration.PublishLeadChangedAsync("lead.deleted", new { id }, cancellationToken);
        return deleted;
    }
}
