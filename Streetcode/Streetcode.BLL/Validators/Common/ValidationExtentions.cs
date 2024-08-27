using FluentValidation;
using FluentValidation.Validators;

namespace Streetcode.BLL.Validators.Common;

public static class ValidationExtentions
{
    public static IRuleBuilderOptions<T, string?> MustBeValidUrl<T>(this IRuleBuilder<T, string?> ruleBuilder, IEnumerable<string>? hosts = null)
    {
        return ruleBuilder.Must(url =>
        {
            if (url == null)
            {
                return false;
            }

            bool isUrl = Uri.TryCreate(url, UriKind.Absolute, out var uriResult);
            if (uriResult != null)
            {
                bool isValidScheme = uriResult!.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps;
                bool isValidHost = true;
                if (hosts != null)
                {
                    isValidHost = hosts!.Any(host => uriResult.Host == host);
                }

                return isUrl && isValidScheme && isValidHost;
            }

            return false;
        }).WithMessage("The {PropertyName} must be valid url");
    }
}