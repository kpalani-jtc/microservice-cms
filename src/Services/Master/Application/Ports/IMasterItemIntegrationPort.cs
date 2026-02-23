namespace Master.Application.Ports;

public interface IMasterItemIntegrationPort
{
    Task PublishChangedAsync(string eventName, object payload, CancellationToken cancellationToken = default);
}
