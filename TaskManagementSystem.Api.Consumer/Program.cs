using TaskManagementSystem.Application;
using TaskManagementSystem.Infrastructure;
using TaskManagementSystem.Infrastructure.Messaging.RabbitMQ;

static IConfigurationRoot BuildConfiguration(IServiceCollection services)
{
    var configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .Build();

    services.AddSingleton<IConfiguration>(configuration);
    return configuration;
}

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        var configuration = BuildConfiguration(services);
       
        services.Configure<RabbitMqOptions>(configuration.GetSection(RabbitMqOptions.SectionName));
        services.AddApplicationLayer();
        services.AddPersistence(configuration);
        services.AddHostedService<RabbitMqConsumerService>();
    })
    .Build();

await host.RunAsync();