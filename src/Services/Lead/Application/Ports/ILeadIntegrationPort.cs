namespace Lead.Application.Ports;

public interface ILeadIntegrationPort
{
    Task PublishLeadChangedAsync(string eventName, object payload, CancellationToken cancellationToken = default);
}
