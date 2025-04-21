using System.Linq.Expressions;
using Streetcode.BLL.Interfaces.EntityAccessManager;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.Services.EntityAccessManager;

public class StreetcodeAccessManager : IEntityAccessManager<StreetcodeContent>
{
    public Expression<Func<StreetcodeContent, bool>>? GetAccessPredicate(UserRole? userRole)
    {
        if (userRole is null or UserRole.User)
        {
            return stc => stc.Status == StreetcodeStatus.Published;
        }

        return null;
    }
}