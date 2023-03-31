using System.Text.Json;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace TaskManagementSystem.Infrastructure.Messaging.RabbitMQ;

public class RabbitMqConsumerService : BackgroundService
{
    private readonly IOptions<RabbitMqOptions> _options;
    private readonly ConnectionFactory _factory;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public RabbitMqConsumerService(IOptions<RabbitMqOptions> options, IServiceScopeFactory serviceScopeFactory)
    {
        _options = options;
        _serviceScopeFactory = serviceScopeFactory;

        _factory = new ConnectionFactory
        {
            HostName = _options.Value.HostName,
            UserName = _options.Value.Username,
            Password = _options.Value.Password,
        };
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        var connection = _factory.CreateConnection();
        var channel = connection.CreateModel();
        channel.BasicQos(0, 0, false);

        foreach (var queueName in new[] {"update_task"})
        {
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                try
                {
                    var messageType = Type.GetType(ea.BasicProperties.Type);
                    var messageBody = ea.Body.ToArray();
                    var message = JsonSerializer.Deserialize(messageBody, messageType!);

                    await using var scope = _serviceScopeFactory.CreateAsyncScope();
                    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                    await mediator.Send(message!, stoppingToken);

                    channel.BasicAck(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error occured while consumed message: {ex.Message}");
                    channel.BasicReject(ea.DeliveryTag, false);
                }
            };

            channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
        }

        return Task.CompletedTask;
    }
}