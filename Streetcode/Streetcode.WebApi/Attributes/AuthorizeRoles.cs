using Microsoft.AspNetCore.Authorization;
using Streetcode.DAL.Enums;
namespace Streetcode.WebApi.Attributes
{
    public class AuthorizeRoles : AuthorizeAttribute
    {
        public AuthorizeRoles(params UserRole[] roles)
        {
            Roles = string.Join(",", roles.Select(r => r.ToString()).ToArray());
        }
    }
}
