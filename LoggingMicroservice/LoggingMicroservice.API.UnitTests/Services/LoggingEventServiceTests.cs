namespace LoggingMicroservice.API.UnitTests.Services;

using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Config;
using Entities;
using FluentAssertions;
using Helpers;
using LoggingMicroservice.API.Services;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

[TestClass]
public class LoggingEventServiceTests
{
    private const string QueueName = "logging.queue.name";
    private const string Environment = "environment";
    private const string Version = "1.0.0";
    private const string Application = "EPR";

    private LoggingEventService _systemUnderTest;
    private Mock<ServiceBusClient> _serviceBusClient;
    private Mock<ServiceBusSender> _serviceBusSender;

    [TestInitialize]
    public void SetUp()
    {
        _serviceBusClient = new Mock<ServiceBusClient>();
        _serviceBusSender = new Mock<ServiceBusSender>();
        _serviceBusClient.Setup(x => x.CreateSender(It.IsAny<string>()))
            .Returns(_serviceBusSender.Object);

        var protectiveMonitoringOptions = new ProtectiveMonitoringOptions
            { Application = Application, Environment = Environment };
        var serviceBusOptions = new ServiceBusOptions
            { QueueName = QueueName, MaxRetries = 3, RetryMaxDelaySeconds = 10, RetryDelaySeconds = 10 };
        var protectiveMonitoringOptionsMock = new Mock<IOptions<ProtectiveMonitoringOptions>>();
        protectiveMonitoringOptionsMock.Setup(x => x.Value).Returns(protectiveMonitoringOptions);
        var serviceBusOptionsMock = new Mock<IOptions<ServiceBusOptions>>();
        serviceBusOptionsMock.Setup(x => x.Value).Returns(serviceBusOptions);

        _systemUnderTest = new LoggingEventService(_serviceBusClient.Object, serviceBusOptionsMock.Object, protectiveMonitoringOptionsMock.Object);
    }

    [TestMethod]
    public async Task CreateEventAsync_SendsMessageToServiceBus_WhenCalled()
    {
        // Arrange
        var loggingEvent = LoggingEventHelpers.CreateLoggingEvent();
        var expected = ServiceBusMessageHelpers.CreateServiceBusMessage(loggingEvent, Environment, Version, Application);

        // Act
        await _systemUnderTest.CreateEventAsync(loggingEvent);

        // Assert
        _serviceBusClient.Verify(x => x.CreateSender(QueueName), Times.Once);
        _serviceBusSender.Verify(
            x => x.SendMessageAsync(
                It.Is<ServiceBusMessage>(p => VerifyServiceBusMessageMatches(p.Body, expected)),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [TestMethod]
    public async Task CreateEventAsync_Throws_WhenServiceBusThrows()
    {
        // Arrange
        var loggingEvent = LoggingEventHelpers.CreateLoggingEvent();
        _serviceBusClient.Setup(x => x.CreateSender(It.IsAny<string>()))
            .Throws<Exception>();

        // Act & Assert
        Func<Task> createEvent = async () => await _systemUnderTest.CreateEventAsync(loggingEvent);
        createEvent.Should().ThrowAsync<Exception>();
        _serviceBusClient.Verify(x => x.CreateSender(QueueName), Times.Once);
    }

    private static bool VerifyServiceBusMessageMatches(BinaryData body, LoggingMessage expected) =>
        body.ToString().Equals(JsonSerializer.Serialize(expected));
}