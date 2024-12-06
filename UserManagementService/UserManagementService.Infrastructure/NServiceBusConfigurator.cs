using Microsoft.Extensions.DependencyInjection;
using NServiceBus;

namespace UserManagementService.Infrastructure;

public static class NServiceBusConfigurator
{
    public static async Task<IEndpointInstance> ConfigureEndpoint(string endpointName, IServiceCollection services)
    {
        var endpointConfiguration = new EndpointConfiguration(endpointName);

        // Configure LearningTransport (in-memory transport for development/testing)
        var transport = endpointConfiguration.UseTransport<LearningTransport>();

        // Configure persistence
        endpointConfiguration.UsePersistence<LearningPersistence>();

        // Enable installers
        endpointConfiguration.EnableInstallers();

        var endpointInstance = await Endpoint.Start(endpointConfiguration).ConfigureAwait(false);

        // Register IMessageSession with the DI container
        services.AddSingleton<IMessageSession>(endpointInstance);

        return endpointInstance;
    }
}