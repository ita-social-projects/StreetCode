namespace Streetcode.BLL.Validators.Common;

public class UrlValidator
{
    public static bool IsValid(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out _);
    }
}