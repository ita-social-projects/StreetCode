using System.Reflection;
using Streetcode.BLL.DTO.Team;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Entities.Team;
using Streetcode.XIntegrationTest.Base;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.AdditionalContent;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.MediaExtracter.Image;
using Xunit.Sdk;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Team
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class ExtractDeleteTeamMemberAttribute : BeforeAfterTestAttribute
    {
        public static UpdateTeamMemberDTO TeamMemberForTest { get; set; } = null!;

        private TeamMember TeamMember { get; set; } = null!;

        private Image Image { get; set; } = null!;

        public override void Before(MethodInfo methodUnderTest)
        {
            int uniqueId = UniqueNumberGenerator.GenerateInt();
            Image = ImageExtracter.Extract(uniqueId);
            TeamMember = TeamMemberExtracter.Extract(uniqueId, Image.Id);

            TeamMemberForTest = new UpdateTeamMemberDTO
            {
                Id = TeamMember.Id,
                Name = "test",
                Description = "test description",
                IsMain = false,
                ImageId = Image.Id,
            };
        }

        public override void After(MethodInfo methodUnderTest)
        {
            var sqlDbHelper = BaseControllerTests.GetSqlDbHelper();
            var context = sqlDbHelper.GetExistItem<TeamMember>(t => t.Id == TeamMemberForTest.Id);
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

            var teamMember = sqlDbHelper.GetExistItem<TeamMember>(t => t.Id == TeamMember.Id);
            if (teamMember != null)
            {
                sqlDbHelper.DeleteItem(teamMember);
                sqlDbHelper.SaveChanges();
            }
        }
    }
}
