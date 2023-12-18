﻿namespace LoggingMicroservice.API.Attributes;

using System.ComponentModel.DataAnnotations;

public class NotDefaultAttribute : ValidationAttribute
{
    public const string DefaultErrorMessage = "The {0} field is missing or has the default value";

    public NotDefaultAttribute()
        : base(DefaultErrorMessage)
    {
    }

    public override bool IsValid(object? value)
    {
        if (value is null)
        {
            return true;
        }

        var type = value.GetType();
        if (type.IsValueType)
        {
            var defaultValue = Activator.CreateInstance(type);
            return !value.Equals(defaultValue);
        }

        return true;
    }
}