using System.Text.RegularExpressions;

namespace TaskManagementSystem.Infrastructure.Messaging.RabbitMQ;

public static class RabbitMqExtensions
{
    public static string GetQueueName(this string messageType)
    {
        var snakeCase = Regex.Replace(messageType, "(?<=.)([A-Z])", "_$1", RegexOptions.Compiled);
        return snakeCase.ToLowerInvariant();
    }
}