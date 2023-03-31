using TaskManagementSystem.Application.Abstractions;

namespace TaskManagementSystem.Infrastructure.Messaging;

public interface IServiceBus
{
    Task SendMessageAsync<TMessage>(TMessage message, CancellationToken cancellationToken = default)
        where TMessage : AsyncCommand;
}