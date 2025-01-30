using System.Reflection;
using Streetcode.BLL.DTO.Team;
using Streetcode.DAL.Entities.Team;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;
using Xunit.Sdk;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Team
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class ExtractCreateTeamLinkAttribute : BeforeAfterTestAttribute
    {
        public static TeamMemberLinkCreateDTO TeamLinkForTest { get; set; } = null!;

        public override void Before(MethodInfo methodUnderTest)
        {
            TeamLinkForTest = new TeamMemberLinkCreateDTO
            {
                LogoType = DAL.Enums.LogoType.Instagram,
                TargetUrl = "https://instagram.com/testCreate",
            };
        }

        public override void After(MethodInfo methodUnderTest)
        {
            var sqlDbHelper = BaseControllerTests.GetSqlDbHelper();
            var context = sqlDbHelper.GetExistItem<TeamMemberLink>(t => t.TargetUrl == TeamLinkForTest.TargetUrl);
            if (context != null)
            {
                sqlDbHelper.DeleteItem(context);
                sqlDbHelper.SaveChanges();
            }
        }
    }
}
