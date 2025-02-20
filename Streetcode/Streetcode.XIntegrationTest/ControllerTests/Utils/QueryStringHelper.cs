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

        var properties = typeof(T).GetProperties()
            .Where(p => p.GetValue(obj) != null)
            .Select(p => $"{HttpUtility.UrlEncode(p.Name)}={HttpUtility.UrlEncode(p.GetValue(obj)?.ToString() ?? string.Empty)}")
            .ToList();

        var queryString = string.Join("&", properties);

        return queryString.Any()
            ? queryString.Insert(0, "?")
            : string.Empty;
    }
}