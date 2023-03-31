using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using TaskManagementSystem.Application.Abstractions;
using TaskManagementSystem.Application.UpdateTask;
using TaskManagementSystem.Infrastructure.Messaging;
using TaskManagementSystem.Infrastructure.Messaging.RabbitMQ;
namespace TaskManagementSystem.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMessaging(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RabbitMqOptions>(configuration.GetSection(RabbitMqOptions.SectionName));
        services.AddTransient<IServiceBus, RabbitMqServiceBus>();
        
        var serviceProvider = services.BuildServiceProvider();
        var options = serviceProvider.GetRequiredService<IOptions<RabbitMqOptions>>();

        var factory = new ConnectionFactory
        {
            UserName = options.Value.Username,
            Password = options.Value.Password,
            HostName = options.Value.HostName
        };
        
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.ExchangeDeclare(exchange: options.Value.Exchange, type: ExchangeType.Direct, durable: true, autoDelete: false);
        DeclareQueues(channel, options.Value.Exchange, typeof(UpdateTask).Assembly);
       
        return services;
    }
    
    private static void DeclareQueues(IModel channel, string exchange, Assembly messagesAssembly)
    {
        foreach (var messageType in GetMessageTypes(typeof(AsyncCommand), messagesAssembly))
        {
            var queueName = messageType.Name.GetQueueName();
            var routingKey = queueName;

            channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
            channel.QueueBind(queue: queueName, exchange: exchange, routingKey: routingKey);
        }
    }
    
    private static IEnumerable<Type> GetMessageTypes(Type baseType, Assembly messagesAssembly)
    {
        var derivedTypes = messagesAssembly.GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false } && baseType.IsAssignableFrom(t));

        return derivedTypes;
    }
}