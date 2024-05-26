using LoggingMicroservice.API.Extensions;
using LoggingMicroservice.API.HealthChecks;
using LoggingMicroservice.API.Services;
using LoggingMicroservice.API.Services.Interfaces;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services
    .AddApplicationInsightsTelemetry();

builder.Services.AddTransient<ILoggingEventService, LoggingEventService>();
builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.ConfigureOptions(configuration);
builder.Services.RegisterAzureServiceBus();

builder.Services.AddApiVersioning();
builder.Services.AddControllers();
builder.Services.AddHealthChecks();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHealthChecks();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/admin/health", HealthCheckOptionsBuilder.Build());
app.MapHealthChecks("/admin/error", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    ResultStatusCodes =
    {
        [HealthStatus.Healthy] = StatusCodes.Status500InternalServerError
    }
}).AllowAnonymous();

app.Run();
