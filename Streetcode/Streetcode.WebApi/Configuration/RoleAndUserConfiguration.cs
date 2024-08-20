using Microsoft.AspNetCore.Identity;
using Streetcode.DAL.Persistence;
using Streetcode.DAL.Entities.Users;
using Streetcode.DAL.Enums;
using static Streetcode.WebApi.Utils.Constants.UserDatabaseSeedingConstants;

namespace Streetcode.WebApi.Configuration
{
    public static class RoleAndUserConfiguration
    {
        public static async Task AddUsersAndRoles(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetService<StreetcodeDbContext>() !;

            // Create roles in database.
            await AddRolesAsync(serviceProvider);

            // Populate initial admin with information.
            var initialAdmin = new User
            {
                Name = "admin",
                Surname = "admin",
                Email = AdminEmail,
                NormalizedEmail = AdminEmail.ToUpper(),
                UserName = AdminUsername,
                NormalizedUserName = AdminUsername.ToUpper(),
                PhoneNumber = "777-777-77-77",
                EmailConfirmed = false,
                PhoneNumberConfirmed = false,
                SecurityStamp = Guid.NewGuid().ToString("D")
            };

            // Create hashed password.
            var password = new PasswordHasher<User>();
            var passwordEnvVariable = Environment.GetEnvironmentVariable("ADMIN_PASSWORD");

            if (passwordEnvVariable is null)
            {
                throw new ArgumentNullException(nameof(passwordEnvVariable), "Password in environment variables is not set and equals null.");
            }

            var hashed = password.HashPassword(initialAdmin, passwordEnvVariable);
            initialAdmin.PasswordHash = hashed;

            // Add initial admin.
            await context.Users.AddAsync(initialAdmin);
            await context.SaveChangesAsync();

            // Assign role 'Admin' to initialAdmin.
            await AssignRole(serviceProvider, initialAdmin.Email, nameof(UserRole.Admin));
            await context.SaveChangesAsync();
        }

        private static async Task AddRolesAsync(IServiceProvider services)
        {
            RoleManager<IdentityRole> roleManager = services.GetService<RoleManager<IdentityRole>>() !;
            if (!roleManager.RoleExistsAsync(nameof(UserRole.Admin)).GetAwaiter().GetResult())
            {
                await roleManager.CreateAsync(new IdentityRole(nameof(UserRole.Admin)));
                await roleManager.CreateAsync(new IdentityRole(nameof(UserRole.User)));
            }
        }

        private static async Task AssignRole(IServiceProvider services, string email, string role)
        {
            UserManager<User> userManager = services.GetService<UserManager<User>>() !;
            User? user = await userManager!.FindByEmailAsync(email);
            if (user is not null)
            {
                await userManager.AddToRoleAsync(user, role);
            }
        }
    }
}
