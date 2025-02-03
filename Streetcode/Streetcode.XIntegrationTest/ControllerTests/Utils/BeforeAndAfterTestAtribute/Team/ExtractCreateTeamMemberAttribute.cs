using System.Reflection;
using Streetcode.BLL.DTO.Team;
using Streetcode.DAL.Entities.Team;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;
using Xunit.Sdk;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Team
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class ExtractCreateTeamMemberAttribute : BeforeAfterTestAttribute
    {
        public static TeamMemberCreateDTO TeamMemberForTest { get; set; } = null!;

        public override void Before(MethodInfo methodUnderTest)
        {
            TeamMemberForTest = new TeamMemberCreateDTO
            {
                Name = "test create",
                Description = "test create description",
                IsMain = false,
            };
        }

        public override void After(MethodInfo methodUnderTest)
        {
            var sqlDbHelper = BaseControllerTests.GetSqlDbHelper();
            var context = sqlDbHelper.GetExistItem<TeamMember>(t => t.Name == TeamMemberForTest.Name);
            if (context != null)
            {
                sqlDbHelper.DeleteItem(context);
                sqlDbHelper.SaveChanges();
            }
        }
    }
}
