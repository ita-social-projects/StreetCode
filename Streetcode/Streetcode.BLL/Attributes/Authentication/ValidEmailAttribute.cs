using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

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

            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.(com|net|org|gov|ua)$", RegexOptions.None, TimeSpan.FromMilliseconds(100)))
            {
                return new ValidationResult("Incorrect email address format");
            }

            return ValidationResult.Success;
        }
    }
}
