using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Streetcode.BLL.Util.Helpers;

public static class HttpContextHelper
{
    public static string GetCurrentUserName(IHttpContextAccessor httpContextAccessor)
    {
        var userName = httpContextAccessor.HttpContext?.User?.Identity?.Name;
        if (string.IsNullOrEmpty(userName))
        {
            throw new UnauthorizedAccessException("User is not authenticated or username is not available.");
        }

        return userName;
    }

    public static string GetCurrentUserId(IHttpContextAccessor httpContextAccessor)
    {
        var userId = httpContextAccessor.HttpContext?.User?.Claims
                    .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
             throw new UnauthorizedAccessException("User ID claim is not available.");
        }

        return userId;
    }

    public static string GetCurrentUserEmail(IHttpContextAccessor httpContextAccessor)
    {
        var email = httpContextAccessor.HttpContext?.User?.Claims
                          .FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        if (string.IsNullOrEmpty(email))
        {
            throw new UnauthorizedAccessException("Email claim is not available.");
        }

        return email;
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