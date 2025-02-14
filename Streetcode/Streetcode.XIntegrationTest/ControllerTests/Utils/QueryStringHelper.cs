using System.Web;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils;

public static class QueryStringHelper<T>
{
    public static string ToQueryString(T obj)
    {
        if (obj is null)
        {
            return string.Empty;
        }

        var properties = obj.GetType().GetProperties()
            .Where(p => p.GetValue(obj) != null)
            .Select(p => $"{HttpUtility.UrlEncode(p.Name)}={HttpUtility.UrlEncode(p.GetValue(obj)?.ToString())}");

        var enumerable = properties as string[] ?? properties.ToArray();
        return enumerable.Any() ? "?" + string.Join("&", enumerable) : string.Empty;
    }
}