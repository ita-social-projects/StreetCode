using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Streetcode.BLL.Attributes.Authentication
{
    public class StrongPasswordAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            string? password = value as string;

            if (password is null)
            {
                return new ValidationResult("Attribute cannot be applied to non-string property");
            }

            if (Regex.Matches(password, @"[\s]").Any())
            {
                return new ValidationResult("Password cannot contain whitespaces");
            }

            if (!Regex.Matches(password, @"\d").Any())
            {
                return new ValidationResult("Password must contain digit");
            }

            if (!Regex.Matches(password, @"[^a-zA-Z\d]").Any())
            {
                return new ValidationResult("Password must contain non-alphanumeric symbol");
            }

            if (password.Contains('%'))
            {
                return new ValidationResult("Password cannot contain '%'");
            }

            if (!Regex.Matches(password, @"\p{Lu}").Any())
            {
                return new ValidationResult("Password must contain UPPERCASE letter");
            }

            if (!Regex.Matches(password, @"\p{Ll}").Any())
            {
                return new ValidationResult("Password must contain lowercase letter");
            }

            return ValidationResult.Success;
        }
    }
}
