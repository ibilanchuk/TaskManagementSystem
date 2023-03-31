namespace TaskManagementSystem.Infrastructure.Messaging.RabbitMQ;

public class RabbitMqOptions
{
    public const string SectionName = "RabbitMq";
    public string Username { get; init; }
    public string Password { get; init; }
    public string HostName { get; set; }
    public string Exchange { get; set; }
}