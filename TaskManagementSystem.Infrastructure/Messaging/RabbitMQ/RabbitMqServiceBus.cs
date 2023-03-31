using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using TaskManagementSystem.Application.Abstractions;

namespace TaskManagementSystem.Infrastructure.Messaging.RabbitMQ;

public class RabbitMqServiceBus : IServiceBus
{
    private readonly IOptions<RabbitMqOptions> _options;
    private readonly ConnectionFactory _factory;

    public RabbitMqServiceBus(IOptions<RabbitMqOptions> options)
    {
        _options = options;
        _factory = new ConnectionFactory
        {
            HostName = options.Value.HostName,
            UserName = options.Value.Username,
            Password = options.Value.Password
        };
    }
    
    public Task SendMessageAsync<TMessage>(TMessage message,
        CancellationToken cancellationToken = default) where TMessage : AsyncCommand
    {
        using var connection = _factory.CreateConnection();
        using var channel = connection.CreateModel();

        var serializedMessage = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(serializedMessage);

        var properties = channel.CreateBasicProperties();
        properties.Persistent = true;
        properties.Type = typeof(TMessage).AssemblyQualifiedName;

        channel.BasicPublish(exchange: _options.Value.Exchange, routingKey: message.GetType().Name.GetQueueName(), 
            basicProperties: properties, body: body);
        
        return Task.CompletedTask;
    }
}