using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Streetcode.BLL.Util.Helpers;

public static class HttpContextHelper
{
    public static string? GetCurrentUserName(IHttpContextAccessor httpContextAccessor)
    {
        return httpContextAccessor.HttpContext?.User?.Identity?.Name;
    }

    public static string? GetCurrentUserId(IHttpContextAccessor httpContextAccessor)
    {
        return httpContextAccessor.HttpContext?.User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
    }
}