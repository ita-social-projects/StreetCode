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

        public Task RegisterAsync(User User, string password)
        {
            throw new NotImplementedException();
        }
    }
}
