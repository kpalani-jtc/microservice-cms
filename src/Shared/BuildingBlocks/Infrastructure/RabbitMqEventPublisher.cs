using System.Text;
using System.Text.Json;
using BuildingBlocks.Application;
using RabbitMQ.Client;

namespace BuildingBlocks.Infrastructure;

public sealed class RabbitMqEventPublisher : IEventPublisher, IDisposable
{
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public RabbitMqEventPublisher(string hostName)
    {
        var factory = new ConnectionFactory { HostName = hostName };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
    }

    public Task PublishAsync<T>(string topic, T payload, CancellationToken cancellationToken = default)
    {
        _channel.ExchangeDeclare("cms.events", ExchangeType.Topic, durable: true);
        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(payload));
        _channel.BasicPublish("cms.events", topic, null, body);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _channel.Dispose();
        _connection.Dispose();
    }
}
