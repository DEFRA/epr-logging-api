namespace LoggingMicroservice.API.UnitTests.Helpers;

using Entities;

public static class LoggingEventHelpers
{
    public static LoggingEvent CreateLoggingEvent()
    {
        return new(
            Guid.NewGuid(),
            Guid.NewGuid(),
            DateTime.Now,
            "testComponent",
            "127.0.0.1",
            "01-02",
            1,
            new("EPR_ANTIVIRUS_THREAT_DETECTED", "A message", "Additional Info"));
    }
}