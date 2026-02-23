using BuildingBlocks.Application;
using ServiceAppointments.Application.Ports;

namespace ServiceAppointments.Infrastructure;

public sealed class ServiceAppointmentIntegrationAdapter : IServiceAppointmentIntegrationPort
{
    private readonly IEventPublisher _eventPublisher;

    public ServiceAppointmentIntegrationAdapter(IEventPublisher eventPublisher)
    {
        _eventPublisher = eventPublisher;
    }

    public Task PublishChangedAsync(string eventName, object payload, CancellationToken cancellationToken = default)
        => _eventPublisher.PublishAsync(eventName, payload, cancellationToken);
}
