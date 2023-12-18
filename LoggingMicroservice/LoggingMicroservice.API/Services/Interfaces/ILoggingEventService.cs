namespace LoggingMicroservice.API.Services.Interfaces;

using Entities;

public interface ILoggingEventService
{
    Task CreateEventAsync(LoggingEvent loggingEvent);
}