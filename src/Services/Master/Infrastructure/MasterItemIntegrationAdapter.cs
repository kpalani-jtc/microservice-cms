using BuildingBlocks.Application;
using Master.Application.Ports;

namespace Master.Infrastructure;

public sealed class MasterItemIntegrationAdapter : IMasterItemIntegrationPort
{
    private readonly IEventPublisher _eventPublisher;

    public MasterItemIntegrationAdapter(IEventPublisher eventPublisher)
    {
        _eventPublisher = eventPublisher;
    }

    public Task PublishChangedAsync(string eventName, object payload, CancellationToken cancellationToken = default)
        => _eventPublisher.PublishAsync(eventName, payload, cancellationToken);
}
