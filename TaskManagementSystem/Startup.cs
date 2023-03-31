using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskManagementSystem.Application;
using TaskManagementSystem.Infrastructure;

namespace TaskManagementSystem;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        var configuration = BuildConfiguration(services);
        
        services.AddApplicationLayer();
        services.AddMessaging(configuration);
        services.AddPersistence(configuration);
    }

    private static IConfigurationRoot BuildConfiguration(IServiceCollection services)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        services.AddSingleton<IConfiguration>(configuration);
        return configuration;
    }
}