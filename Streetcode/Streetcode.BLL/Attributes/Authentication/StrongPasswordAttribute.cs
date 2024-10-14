using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Streetcode.BLL.Attributes.Authentication
{
    public class StrongPasswordAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is null)
            {
                return new ValidationResult("Input parameter cannot be null");
            }

            string? password = value as string;

            if (password is null)
            {
                return new ValidationResult("Attribute cannot be applied to non-string property");
            }

            if (password.Length < 14)
            {
                return new ValidationResult("Password minimum length is 14");
            }

            if (Regex.Matches(password, @"[\s]", RegexOptions.None, TimeSpan.FromMilliseconds(100)).Any())
            {
                return new ValidationResult("Password cannot contain whitespaces");
            }

            if (!Regex.Matches(password, @"\d", RegexOptions.None, TimeSpan.FromMilliseconds(100)).Any())
            {
                return new ValidationResult("Password must contain at least one digit");
            }

            if (!Regex.Matches(password, @"[^a-zA-Z\d]", RegexOptions.None, TimeSpan.FromMilliseconds(100)).Any())
            {
                return new ValidationResult("Password must contain at least one non-alphanumeric symbol");
            }

            if (password.Contains('%'))
            {
                return new ValidationResult("Password cannot contain '%'");
            }

            if (!Regex.Matches(password, @"\p{Lu}", RegexOptions.None, TimeSpan.FromMilliseconds(100)).Any())
            {
                return new ValidationResult("Password must contain at least one UPPERCASE letter");
            }

            if (!Regex.Matches(password, @"\p{Ll}", RegexOptions.None, TimeSpan.FromMilliseconds(100)).Any())
            {
                return new ValidationResult("Password must contain at least one lowercase letter");
            }

            return ValidationResult.Success;
        }
    }
}
