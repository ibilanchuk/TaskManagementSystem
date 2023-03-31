using MediatR;
using Microsoft.Extensions.DependencyInjection;
using TaskManagementSystem.Application.UpdateTask;

namespace TaskManagementSystem.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
    {
        services.AddMediatR(typeof(UpdateTaskHandler).Assembly);
        
        return services;
    }
}