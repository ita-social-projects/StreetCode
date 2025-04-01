using System.Linq.Expressions;
using Streetcode.BLL.Interfaces.EntityAccessManager;
using Streetcode.DAL.Entities.News;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.Services.EntityAccessManager;

public class NewsAccessManager : IEntityAccessManager<News>
{
    public Expression<Func<News, bool>>? GetAccessPredicate(UserRole? userRole)
    {
        if (userRole is null or UserRole.User)
        {
            return news => news.CreationDate < DateTime.UtcNow;
        }

        return null;
    }
}