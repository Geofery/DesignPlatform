using Microsoft.Extensions.DependencyInjection;
using NServiceBus;

namespace UserManagementService.Infrastructure;

public static class NServiceBusConfigurator
{
    [Obsolete]
    public static IEndpointInstance ConfigureAndStartEndpoint(string endpointName, IServiceCollection services)
    {
        // Create Endpoint Configuration
        var endpointConfiguration = new EndpointConfiguration(endpointName);

        // Use Learning Transport (for development)
        var transport = endpointConfiguration.UseTransport<LearningTransport>();
        transport.StorageDirectory("LearningTransportStorage"); // Optional: Set storage directory for messages

        // Use Learning Persistence (for development)
        endpointConfiguration.UsePersistence<LearningPersistence>();

        // Enable Installers for creating queues, etc. (for development)
        endpointConfiguration.EnableInstallers();

        // Recoverability Configuration
        var recoverability = endpointConfiguration.Recoverability();
        recoverability.Delayed(retries =>
        {
            retries.NumberOfRetries(3);
            retries.TimeIncrease(TimeSpan.FromSeconds(10));
        });
        recoverability.Immediate(retries =>
        {
            retries.NumberOfRetries(5);
        });

        // Register custom dependency injection container
        var serviceProvider = services.BuildServiceProvider();
        endpointConfiguration.RegisterComponents(components =>
        {
            foreach (var service in services)
            {
                components.ConfigureComponent(service.ServiceType, DependencyLifecycle.InstancePerCall);
            }
        });


        // Start the Endpoint
        var endpointInstance = Endpoint.Start(endpointConfiguration).GetAwaiter().GetResult();

        // Register IMessageSession in the DI Container
        services.AddSingleton<IMessageSession>(endpointInstance);

        return endpointInstance;
    }
}
