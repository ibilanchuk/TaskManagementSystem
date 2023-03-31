using Microsoft.Extensions.DependencyInjection;

namespace TaskManagementSystem.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
    {
        return services;
    }

}