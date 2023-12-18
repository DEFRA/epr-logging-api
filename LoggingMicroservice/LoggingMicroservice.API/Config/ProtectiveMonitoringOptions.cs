namespace LoggingMicroservice.API.Config;

using System.ComponentModel.DataAnnotations;

public class ProtectiveMonitoringOptions
{
    public const string Section = "ProtectiveMonitoring";

    [Required]
    public string Environment { get; init; }

    [Required]
    public string Application { get; init; }
}