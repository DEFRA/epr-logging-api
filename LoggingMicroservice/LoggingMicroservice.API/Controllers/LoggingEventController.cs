namespace LoggingMicroservice.API.Controllers;

using System.Net;
using Asp.Versioning;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/log-events")]
[SwaggerTag]
public class LoggingEventController : ControllerBase
{
    private readonly ILoggingEventService _loggingEventService;
    private readonly ILogger<LoggingEventController> _logger;

    public LoggingEventController(
        ILoggingEventService loggingEventService,
        ILogger<LoggingEventController> logger)
    {
        _loggingEventService = loggingEventService;
        _logger = logger;
    }

    [HttpPost(Name = "Create Event")]
    [Consumes("application/json")]
    public async Task<IActionResult> CreateEvent([FromBody] LoggingEvent request)
    {
        try
        {
            await _loggingEventService.CreateEventAsync(request);
            _logger.LogInformation("A log event created successfully");
            return new StatusCodeResult((int)HttpStatusCode.Created);
        }
        catch (Exception exception)
        {
            _logger.LogCritical(exception, "An error occurred creating the log event");
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }
}