﻿using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Streetcode.BLL.Attributes.Authentication
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ValidEmailAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is null)
            {
                return new ValidationResult("Input parameter cannot be null");
            }

            string? email = value as string;

            if (email is null)
            {
                return new ValidationResult("Attribute cannot be applied to non-string property");
            }

            if (!Regex.IsMatch(email, @"^(?!.*\.\.)[a-zA-Z0-9_%+-]+(?:\.[a-zA-Z0-9_%+-]+)*@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", RegexOptions.None, TimeSpan.FromMilliseconds(100)))
            {
                return new ValidationResult("Incorrect email address format");
            }

            return ValidationResult.Success;
        }
    }
}
