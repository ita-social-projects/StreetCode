using FluentValidation;
using FluentValidation.Validators;

namespace Streetcode.BLL.Validators.Common;

public static class ValidationExtentions
{
    public static IRuleBuilderOptions<T, string?> MustBeValidUrl<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder.Must(url =>
        {
            if (url == null)
            {
                return false;
            }

            bool result = Uri.TryCreate(url, UriKind.Absolute, out var uriResult);
            return result && (uriResult!.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }).WithMessage("The {PropertyName} must be valid url");
    }
}