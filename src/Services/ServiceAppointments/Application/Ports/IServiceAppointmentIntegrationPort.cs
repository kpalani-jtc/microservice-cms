namespace ServiceAppointments.Application.Ports;

public interface IServiceAppointmentIntegrationPort
{
    Task PublishChangedAsync(string eventName, object payload, CancellationToken cancellationToken = default);
}
