using Microsoft.AspNetCore.Identity;
using Streetcode.DAL.Entities.Users;
using Streetcode.DAL.Enums;

namespace Streetcode.XIntegrationTest.Constants.ControllerTests
{
    internal static class AuthConstants
    {
        public const string TEST_USER_LOGIN_PASSWORD = "User_Login_Password_Q123#";

        public static readonly IdentityRole TEST_ROLE_ADMIN = new IdentityRole()
        {
            Id = "Test_Role_Admin_qwe123456rty#",
            Name = nameof(UserRole.Admin),
            NormalizedName = nameof(UserRole.Admin).ToUpper(),
        };

        public static readonly IdentityRole TEST_ROLE_USER = new IdentityRole()
        {
            Id = "Test_Role_User_qwe123456rty#",
            Name = nameof(UserRole.User),
            NormalizedName = nameof(UserRole.User).ToUpper(),
        };

        public static readonly User TEST_USER_ADMIN = new User()
        {
            Id = "Test_User_Admin_qwe123456rty#",
            Name = "User_Admin",
            Surname = "User_Admin",
            Email = "user@admin.com",
            UserName = "User_Admin_T",
        };

        public static readonly User TEST_USER_USER = new User()
        {
            Id = "Test_User_User_qwe123456rty#",
            Name = "User_User",
            Surname = "User_User",
            Email = "user@user.com",
            UserName = "User_User_T",
        };

        public static readonly User TEST_USER_LOGIN = new User()
        {
            Id = "Test_User_Login_qwe123456rty#",
            Name = "User_Login",
            Surname = "User_Login",
            Email = "user@login.com",
            UserName = "User_Login_T",
        };
    }
}
