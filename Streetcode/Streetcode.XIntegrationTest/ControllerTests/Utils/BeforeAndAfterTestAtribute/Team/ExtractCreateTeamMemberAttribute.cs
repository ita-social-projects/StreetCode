using System.Reflection;
using Streetcode.BLL.DTO.Team;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Entities.Team;
using Streetcode.XIntegrationTest.Base;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.MediaExtracter.Image;
using Xunit.Sdk;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Team
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class ExtractCreateTeamMemberAttribute : BeforeAfterTestAttribute
    {
        public static TeamMemberCreateDTO TeamMemberForTest { get; set; } = null!;

        private Image Image { get; set; } = null!;

        public override void Before(MethodInfo methodUnderTest)
        {
            Image = ImageExtracter.Extract(UniqueNumberGenerator.GenerateInt());
            TeamMemberForTest = new TeamMemberCreateDTO
            {
                Name = "test create",
                Description = "test create description",
                IsMain = false,
                ImageId = Image.Id,
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

            var image = sqlDbHelper.GetExistItem<Image>(t => t.Id == Image.Id);
            if (image != null)
            {
                sqlDbHelper.DeleteItem(image);
                sqlDbHelper.SaveChanges();
            }
        }
    }
}
