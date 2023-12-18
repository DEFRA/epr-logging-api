namespace LoggingMicroservice.API.Config;

using System.ComponentModel.DataAnnotations;

public class ServiceBusOptions
{
    public const string Section = "ServiceBus";

    [Required]
    public string ConnectionString { get; init; }

    [Required]
    public string QueueName { get; init; }

    public double RetryDelaySeconds { get; init; }

    public double RetryMaxDelaySeconds { get; init; }

    public int MaxRetries { get; init; }
}