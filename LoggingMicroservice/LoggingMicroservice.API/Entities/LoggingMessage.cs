namespace LoggingMicroservice.API.Entities;

public record LoggingMessage(
    Guid UserId,
    Guid SessionId,
    DateTime DateTime,
    string Environment,
    string Version,
    string Application,
    string Component,
    string Ip,
    string PmcCode,
    int Priority,
    EventDetails Details)
{
    public LoggingMessage(
        LoggingEvent loggingEvent,
        string environment,
        string version,
        string application)
        : this(
            loggingEvent.UserId,
            loggingEvent.SessionId,
            loggingEvent.DateTime,
            environment,
            version,
            application,
            loggingEvent.Component,
            loggingEvent.Ip,
            loggingEvent.PmcCode,
            loggingEvent.Priority,
            loggingEvent.Details)
    {
    }
}
