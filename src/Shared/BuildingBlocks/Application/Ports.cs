namespace BuildingBlocks.Application;

public interface ICommand<out TResponse> { }
public interface ICommandHandler<in TCommand, TResponse> where TCommand : ICommand<TResponse>
{
    Task<TResponse> Handle(TCommand command, CancellationToken cancellationToken);
}

public interface IEventPublisher
{
    Task PublishAsync<T>(string topic, T payload, CancellationToken cancellationToken = default);
}

public interface IPermissionService
{
    Task<bool> HasPermissionAsync(string subjectId, string permission, CancellationToken cancellationToken = default);
}
