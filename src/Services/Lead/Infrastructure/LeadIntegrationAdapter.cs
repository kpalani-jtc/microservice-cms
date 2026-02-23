using BuildingBlocks.Application;
using Lead.Application.Ports;

namespace Lead.Infrastructure;

public sealed class LeadIntegrationAdapter : ILeadIntegrationPort
{
    private readonly IEventPublisher _eventPublisher;

    public LeadIntegrationAdapter(IEventPublisher eventPublisher)
    {
        _eventPublisher = eventPublisher;
    }

    public Task PublishLeadChangedAsync(string eventName, object payload, CancellationToken cancellationToken = default)
        => _eventPublisher.PublishAsync(eventName, payload, cancellationToken);
}
