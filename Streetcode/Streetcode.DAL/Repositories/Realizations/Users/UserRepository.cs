using Microsoft.AspNetCore.Identity;
using Streetcode.DAL.Entities.Users;
using Streetcode.DAL.Persistence;
using Streetcode.DAL.Repositories.Interfaces.Users;
using Streetcode.DAL.Repositories.Realizations.Base;

namespace Streetcode.DAL.Repositories.Realizations.Users
{
    internal class UserRepository : RepositoryBase<User>, IUserRepository
    {
        private readonly StreetcodeDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserRepository(StreetcodeDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
            : base(context)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task RegisterAsync(User user, string password)
        {
            try
            {
                var result = await _userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "user");
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return;
        }
    }
}
