using Streetcode.DAL.Entities.Users;

namespace Streetcode.DAL.Repositories.Interfaces.Authorization
{
    public interface IAuthRepository
    {
        Task RegisterAsync(User User, string password);
    }
}
