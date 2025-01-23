using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Streetcode.DAL.Entities.Users;
using Streetcode.DAL.Enums;
using Streetcode.DAL.Persistence;
using static Streetcode.WebApi.Utils.Constants.UserDatabaseSeedingConstants;

namespace Streetcode.WebApi.Configuration
{
    public static class RoleAndUserConfiguration
    {
        public static async Task AddUsersAndRoles(IServiceProvider serviceProvider)
        {
            using IServiceScope localScope = serviceProvider.CreateScope();
            var context = localScope.ServiceProvider.GetRequiredService<StreetcodeDbContext>();

            // Create roles in database.
            await AddRolesAsync(localScope.ServiceProvider);

            // Populate initial admin with information.
            var initialAdmin = new User
            {
                Id = Guid.Parse("FFEF6F7A-5054-44B7-B5FC-4AEA42803F0B").ToString(),
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

            context.Entry(initialAdmin).State = EntityState.Added;

            // Add initial admin.
            await context.Users.AddAsync(initialAdmin);
            await context.SaveChangesAsync();

            // Assign role 'Admin' to initialAdmin.
            await AssignRole(localScope.ServiceProvider, initialAdmin.Email, nameof(UserRole.Admin));
            await context.SaveChangesAsync();
        }

        private static async Task AddRolesAsync(IServiceProvider serviceProvider)
        {
            // RoleManager has scoped lifetime
            using IServiceScope localScope = serviceProvider.CreateScope();
            RoleManager<IdentityRole> roleManager = localScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            if (!roleManager.RoleExistsAsync(nameof(UserRole.Admin)).GetAwaiter().GetResult())
            {
                await roleManager.CreateAsync(new IdentityRole(nameof(UserRole.Admin)));
                await roleManager.CreateAsync(new IdentityRole(nameof(UserRole.User)));
            }
        }

        private static async Task AssignRole(IServiceProvider serviceProvider, string email, string role)
        {
            // UserManager has scoped lifetime
            using IServiceScope localScope = serviceProvider.CreateScope();
            UserManager<User> userManager = localScope.ServiceProvider.GetRequiredService<UserManager<User>>();

            User? user = await userManager.FindByEmailAsync(email);

            if (user is null)
            {
                throw new InvalidOperationException($"User with email '{email}' not found.");
            }

            await userManager.AddToRoleAsync(user, role);
        }
    }
}
