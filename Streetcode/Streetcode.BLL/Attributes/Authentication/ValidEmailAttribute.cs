using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Streetcode.BLL.Attributes.Authentication
{
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

            // Check if input email has standart format( e.g. *******@****.com).
            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.(com|net|org|gov|ua)$"))
            {
                return new ValidationResult("Incorrect email address format");
            }

            return ValidationResult.Success;
        }
    }
}
