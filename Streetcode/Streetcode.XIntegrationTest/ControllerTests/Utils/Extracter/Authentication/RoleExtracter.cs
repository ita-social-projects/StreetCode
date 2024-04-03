using Microsoft.AspNetCore.Identity;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.Authentication
{
    public class RoleExtracter
    {
        public static IdentityRole Extract(string roleName)
        {
            IdentityRole testRole = new IdentityRole()
            {
                Name = roleName,
            };

            return BaseExtracter.Extract<IdentityRole>(testRole, role => role.Name == roleName, false);
        }

        public static void AddUserRole(string userId, string roleId)
        {
            IdentityUserRole<string> testUserRole = new IdentityUserRole<string>()
            {
                UserId = userId,
                RoleId = roleId,
            };

            BaseExtracter.Extract<IdentityUserRole<string>>(
                testUserRole,
                userRole => userRole.UserId == userId && userRole.RoleId == roleId,
                false);
        }
    }
}
