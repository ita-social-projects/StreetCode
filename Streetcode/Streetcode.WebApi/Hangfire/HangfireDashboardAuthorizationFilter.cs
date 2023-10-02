using Hangfire.Dashboard;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.Services.Hangfire;

public class HangfireDashboardAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        var user = context.GetHttpContext().User;

        var isAdministrator = user.IsInRole(UserRole.MainAdministrator.ToString());

        return isAdministrator;
    }
}