namespace LoggingMicroservice.API.UnitTests.Helpers;

using Entities;

public static class ServiceBusMessageHelpers
{
    public static LoggingMessage CreateServiceBusMessage(
        LoggingEvent loggingEvent,
        string environment,
        string version,
        string application) =>
        new(loggingEvent, environment, version, application);
}