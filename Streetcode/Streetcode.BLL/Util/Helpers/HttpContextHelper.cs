using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Streetcode.BLL.Util.Helpers;

public static class HttpContextHelper
{
    public static string GetCurrentUserName(IHttpContextAccessor httpContextAccessor)
    {
        return httpContextAccessor.HttpContext?.User?.Identity?.Name!;
    }

    public static string GetCurrentUserId(IHttpContextAccessor httpContextAccessor)
    {
        return httpContextAccessor.HttpContext?.User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value!;
    }

    public static string GetCurrentUserEmail(IHttpContextAccessor httpContextAccessor)
    {
        return httpContextAccessor.HttpContext?.User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value!;
    }

    public static string? GetCurrentDomain(IHttpContextAccessor httpContextAccessor)
    {
        var request = httpContextAccessor.HttpContext?.Request;

        if (request?.Headers.ContainsKey("Origin") == true)
        {
            return request.Headers["Origin"].ToString();
        }

        if (request?.Headers.ContainsKey("Referer") == true)
        {
            return new Uri(request.Headers["Referer"].ToString()).Host;
        }

        return null;
    }
}