namespace LoggingMicroservice.API.UnitTests.Controllers;

using FluentAssertions;
using Helpers;
using LoggingMicroservice.API.Controllers;
using LoggingMicroservice.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

[TestClass]
public class LoggingEventControllerTests
{
    private LoggingEventController _systemUnderTest;
    private Mock<ILoggingEventService> _loggingEventService;
    private Mock<ILogger<LoggingEventController>> _logger;

    [TestInitialize]
    public void TestInitialize()
    {
        _loggingEventService = new Mock<ILoggingEventService>();
        _logger = new Mock<ILogger<LoggingEventController>>();
        _systemUnderTest = new LoggingEventController(_loggingEventService.Object, _logger.Object);
    }

    [TestMethod]
    public async Task CreateEventAsync_ReturnsCreatedResult_WhenCalled()
    {
        // Arrange
        var loggingEvent = LoggingEventHelpers.CreateLoggingEvent();

        // Act
        var result = await _systemUnderTest.CreateEvent(loggingEvent);

        // Assert
        _loggingEventService.Verify(x => x.CreateEventAsync(loggingEvent), Times.Once);
        _logger.VerifyLog(x => x.LogInformation("A log event created successfully"));
        result.As<StatusCodeResult>().StatusCode.Should().Be(201);
    }

    [TestMethod]
    public async Task CreateEventAsync_ReturnsInternalServerError_WhenExceptionOccurs()
    {
        // Arrange
        var loggingEvent = LoggingEventHelpers.CreateLoggingEvent();
        var exception = new Exception();
        _loggingEventService.Setup(x => x.CreateEventAsync(loggingEvent)).ThrowsAsync(exception);

        // Act
        var result = await _systemUnderTest.CreateEvent(loggingEvent);

        // Assert
        _loggingEventService.Verify(x => x.CreateEventAsync(loggingEvent), Times.Once);
        _logger.VerifyLog(x => x.LogCritical(exception, "An error occurred creating the log event"));
        result.As<StatusCodeResult>().StatusCode.Should().Be(500);
    }
}