namespace LoggingMicroservice.API.HealthChecks;

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

[ExcludeFromCodeCoverage]
public static class HealthCheckOptionsBuilder
{
    public static HealthCheckOptions Build()
    {
        return new()
        {
            AllowCachingResponses = false,
            ResultStatusCodes =
            {
                [HealthStatus.Healthy] = StatusCodes.Status200OK
            }
        };
    }
}