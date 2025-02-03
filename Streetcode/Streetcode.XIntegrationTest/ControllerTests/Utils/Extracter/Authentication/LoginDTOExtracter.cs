using Microsoft.AspNetCore.Identity;
using Streetcode.BLL.DTO.Authentication.Login;
using Streetcode.DAL.Entities.Users;
using Streetcode.DAL.Enums;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.Authentication
{
    public class LoginDtoExtracter
    {
        public static LoginRequestDto Extract(string userId, string userEmail, string roleName = nameof(UserRole.User))
        {
            string testPassword = "Test_Password_123";
            User testUser = TestDataProvider.GetTestData<User>();
            testUser.Id = userId;
            testUser.Email = userEmail;
            testUser.PasswordHash = HashPassword(testPassword, testUser);
            BaseExtracter.Extract<User>(testUser, user => user.Id == userId && user.Email == userEmail);

            IdentityRole role = RoleExtracter.Extract(roleName);
            RoleExtracter.AddUserRole(testUser.Id, role.Id);

            return new LoginRequestDto()
            {
                Login = testUser.Email,
                Password = testPassword,
            };
        }

        private static string HashPassword(string password, User user)
        {
            var hasher = new PasswordHasher<User>();
            var hashedPassword = hasher.HashPassword(user, password);
            return hashedPassword;
        }
    }
}
