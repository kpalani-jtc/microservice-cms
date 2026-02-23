using BuildingBlocks.Application;
using Complaints.Application.Ports;

namespace Complaints.Infrastructure;

public sealed class ComplaintIntegrationAdapter : IComplaintIntegrationPort
{
    private readonly IEventPublisher _eventPublisher;

    public ComplaintIntegrationAdapter(IEventPublisher eventPublisher)
    {
        _eventPublisher = eventPublisher;
    }

    public Task PublishChangedAsync(string eventName, object payload, CancellationToken cancellationToken = default)
        => _eventPublisher.PublishAsync(eventName, payload, cancellationToken);
}
