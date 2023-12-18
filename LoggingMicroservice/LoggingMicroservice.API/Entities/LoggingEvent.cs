namespace LoggingMicroservice.API.Entities;

using System.ComponentModel.DataAnnotations;
using Attributes;

public record LoggingEvent(
    [NotDefault]
    Guid UserId,

    [NotDefault]
    Guid SessionId,

    [NotDefault]
    DateTime DateTime,

    [Required]
    string Component,

    [Required]
    string Ip,

    [Required]
    string PmcCode,

    [Required]
    int Priority,

    [Required]
    EventDetails Details);