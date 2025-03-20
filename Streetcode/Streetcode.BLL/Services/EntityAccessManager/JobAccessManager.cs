using System.Linq.Expressions;
using Streetcode.BLL.Interfaces.EntityAccessManager;
using Streetcode.DAL.Entities.Jobs;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.Services.EntityAccessManager;

public class JobAccessManager : IEntityAccessManager<Job>
{
    public Expression<Func<Job, bool>>? GetAccessPredicate(UserRole? userRole)
    {
        if(userRole is null or UserRole.User)
        {
            return job => job.Status == true;
        }

        return null;
    }
}