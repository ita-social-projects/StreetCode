using FluentValidation;
using FluentValidation.Validators;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.Validators.Common;

public static class ValidationExtentions
{
    public static IRuleBuilderOptions<T, string?> MustBeValidUrl<T>(this IRuleBuilder<T, string?> ruleBuilder, IEnumerable<string>? hosts = null)
    {
        return ruleBuilder.Must(url =>
        {
            if (url == null)
            {
                return true;
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

    public static bool MatchLogotypeAndUrl(string url, LogoType? logoType)
    {
        bool isUri = Uri.TryCreate(url, UriKind.Absolute, out var uri);
        if (logoType == null)
        {
            return false;
        }

        if (!isUri || !Enum.IsDefined(typeof(LogoType), logoType))
        {
            return false;
        }

        string host = uri!.Host;
        switch (logoType)
        {
            case LogoType.Behance:
                return host == "www.behance.net" || host == "behance.net";
            case LogoType.Facebook:
                return host == "facebook.com" || host == "www.facebook.com";
            case LogoType.Instagram:
                return host == "instagram.com" || host == "www.instagram.com";
            case LogoType.Linkedin:
                return host == "linkedin.com" || host == "www.linkedin.com";
            case LogoType.Tiktok:
                return host == "tiktok.com" || host == "www.tiktok.com" || host == "vm.tiktok.com";
            case LogoType.Twitter:
                return host == "x.com" || host == "www.x.com";
            case LogoType.YouTube:
                return host == "youtube.com" || host == "www.youtube.com" || host == "youtu.be";
            default:
                throw new ArgumentException("This type of logo is not supported by validation");
        }
    }

    public static string ConcatWithComma(List<string> values)
    {
        if (values.Count < 1)
        {
            return string.Empty;
        }

        var result = values[0];
        result += string.Concat(values.Skip(1).Select(e => $", {e}"));
        return result;
    }
}