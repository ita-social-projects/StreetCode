﻿using Microsoft.AspNetCore.Identity;
using Streetcode.DAL.Entities.Users;
using Streetcode.DAL.Enums;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.Authentication
{
    public class UserExtracter
    {
        public static (User user, string password) Extract(string userId, string password, params string[] roleNames)
        {
            User testUser = TestDataProvider.GetTestData<User>();
            testUser.Id = userId;
            testUser.PasswordHash = HashPassword(password, testUser);
            BaseExtracter.Extract<User>(testUser, user => user.Id == userId, false);
            if (roleNames.Length == 0)
            {
                IdentityRole role = RoleExtracter.Extract(nameof(UserRole.User));
                RoleExtracter.AddUserRole(testUser.Id, role.Id);
                return (testUser, password);
            }

            foreach (var roleName in roleNames)
            {
                IdentityRole role = RoleExtracter.Extract(nameof(UserRole.User));
                RoleExtracter.AddUserRole(testUser.Id, role.Id);
            }

            return (testUser, password);
        }

        public static void Remove(User entity)
        {
            BaseExtracter.RemoveByPredicate<User>(user => user.Id == entity.Id);
        }

        private static string HashPassword(string password, User user)
        {
            var hasher = new PasswordHasher<User>();
            var hashedPassword = hasher.HashPassword(user, password);
            return hashedPassword;
        }
    }
}