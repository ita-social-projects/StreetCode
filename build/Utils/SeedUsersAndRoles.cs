using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Streetcode.DAL.Persistence;
using System.Collections.Generic;
using System.Linq;
using Streetcode.DAL.Entities.Users;

namespace Utils
{
    public static class SeedUsersAndRoles
    {
        private static readonly StreetcodeDbContext _streetcodeDbContext;
        private static readonly Dictionary<string, User> _users;

        static SeedUsersAndRoles()
        {
            _streetcodeDbContext = GetDbContext();
            _users = new Dictionary<string, User>();
        }

        public static void Dispose()
        {
            _streetcodeDbContext.Dispose();
        }

        private static StreetcodeDbContext GetDbContext()
        {
            var optionBuilder = new DbContextOptionsBuilder<StreetcodeDbContext>();
            optionBuilder.UseSqlServer(IntegrationTestsDatabaseConfiguration.ConnectionString);
            return new StreetcodeDbContext(optionBuilder.Options);
        }

        public static void SeedDatabaseWithInitialUsers()
        {
            IdentityRole adminRole = AuthConstants.TEST_ROLE_ADMIN;
            IdentityRole userRole = AuthConstants.TEST_ROLE_USER;
            AddRoleToDb(adminRole);
            AddRoleToDb(userRole);

            _users["Admin"] = AuthConstants.TEST_USER_ADMIN;
            _users["User"] = AuthConstants.TEST_USER_USER;
            AddUserToDb(_users["Admin"]);
            AddUserToDb(_users["User"]);

            AddUserRole(_users["Admin"], adminRole);
            AddUserRole(_users["User"], userRole);
        }

        private static void AddRoleToDb(IdentityRole role)
        {
            bool exists = _streetcodeDbContext.Roles.Any(roleFromDb => roleFromDb.Id == role.Id);
            if (!exists)
            {
                _streetcodeDbContext.Roles.Add(role);
                _streetcodeDbContext.SaveChanges();
            }
        }

        private static void AddUserToDb(User user)
        {
            bool exists = _streetcodeDbContext.Users.Any(userFromDb => userFromDb.Id == user.Id);
            if (!exists)
            {
                _streetcodeDbContext.Users.Add(user);
                _streetcodeDbContext.SaveChanges();
            }
        }

        private static void AddUserRole(User user, IdentityRole role)
        {
            bool exists = _streetcodeDbContext
                .UserRoles
                .Any(userRole => userRole.UserId == user.Id && userRole.RoleId == role.Id);

            if (!exists)
            {
                var actualUserRole = new IdentityUserRole<string>
                {
                    UserId = user.Id,
                    RoleId = role.Id,
                };
                _streetcodeDbContext.UserRoles.Add(actualUserRole);
                _streetcodeDbContext.SaveChanges();
            }
        }
    }
}
