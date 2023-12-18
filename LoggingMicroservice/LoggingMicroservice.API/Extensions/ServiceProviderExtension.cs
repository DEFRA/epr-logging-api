namespace LoggingMicroservice.API.Extensions;

using System.Diagnostics.CodeAnalysis;
using Config;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Options;

[ExcludeFromCodeCoverage]
public static class ServiceProviderExtension
{
    public static void ConfigureOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ServiceBusOptions>(configuration.GetSection(ServiceBusOptions.Section));
        services.Configure<ProtectiveMonitoringOptions>(configuration.GetSection(ProtectiveMonitoringOptions.Section));
    }

    public static void RegisterAzureServiceBus(this IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var serviceBusOptions = serviceProvider.GetRequiredService<IOptions<ServiceBusOptions>>().Value;

        services.AddAzureClients(builder =>
        {
            builder.AddServiceBusClient(serviceBusOptions.ConnectionString)
                .ConfigureOptions(options =>
                {
                    options.RetryOptions.Delay = TimeSpan.FromSeconds(serviceBusOptions.RetryDelaySeconds);
                    options.RetryOptions.MaxDelay = TimeSpan.FromSeconds(serviceBusOptions.RetryMaxDelaySeconds);
                    options.RetryOptions.MaxRetries = serviceBusOptions.MaxRetries;
                });
        });
    }
}
