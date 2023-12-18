namespace LoggingMicroservice.API.Services;

using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Config;
using Entities;
using Interfaces;
using Microsoft.Extensions.Options;

public class LoggingEventService : ILoggingEventService
{
    private readonly ServiceBusClient _serviceBusClient;
    private readonly ServiceBusOptions _serviceBusOptions;
    private readonly ProtectiveMonitoringOptions _protectiveMonitoringOptions;

    public LoggingEventService(
        ServiceBusClient serviceBusClient,
        IOptions<ServiceBusOptions> serviceBusOptions,
        IOptions<ProtectiveMonitoringOptions> protectiveMonitoringOptions)
    {
        _serviceBusClient = serviceBusClient;
        _serviceBusOptions = serviceBusOptions.Value;
        _protectiveMonitoringOptions = protectiveMonitoringOptions.Value;
    }

    public async Task CreateEventAsync(LoggingEvent loggingEvent)
    {
        LoggingMessage message = new(
            loggingEvent,
            _protectiveMonitoringOptions.Environment,
            "1.0.0",
            _protectiveMonitoringOptions.Application);

        var sender = _serviceBusClient.CreateSender(_serviceBusOptions.QueueName);
        var serviceBusMessage = new ServiceBusMessage(JsonSerializer.Serialize(message));

        await sender.SendMessageAsync(serviceBusMessage);
    }
}