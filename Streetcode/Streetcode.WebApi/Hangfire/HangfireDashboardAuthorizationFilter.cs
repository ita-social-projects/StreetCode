using System.Security.Claims;
using Hangfire.Dashboard;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.Services.Hangfire;

public class HangfireDashboardAuthorizationFilter : IDashboardAuthorizationFilter
{
	public bool Authorize(DashboardContext context)
	{
		var user = GetUser(context);
		var isMainAdministrator = IsMainAdministrator(user);

		return isMainAdministrator;
	}

	public bool IsMainAdministrator(ClaimsPrincipal user)
	{
		return user.IsInRole(UserRole.MainAdministrator.ToString());
	}

	public virtual ClaimsPrincipal GetUser(DashboardContext context)
	{
		return context.GetHttpContext().User;
	}
}