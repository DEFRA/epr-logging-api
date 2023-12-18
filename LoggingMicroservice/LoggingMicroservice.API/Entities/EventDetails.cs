namespace LoggingMicroservice.API.Entities;

using System.ComponentModel.DataAnnotations;

public record EventDetails(
    [Required]
    string TransactionCode,

    [Required]
    string Message,

    string AdditionalInfo);