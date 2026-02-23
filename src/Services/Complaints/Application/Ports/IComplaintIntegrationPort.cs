namespace Complaints.Application.Ports;

public interface IComplaintIntegrationPort
{
    Task PublishChangedAsync(string eventName, object payload, CancellationToken cancellationToken = default);
}
