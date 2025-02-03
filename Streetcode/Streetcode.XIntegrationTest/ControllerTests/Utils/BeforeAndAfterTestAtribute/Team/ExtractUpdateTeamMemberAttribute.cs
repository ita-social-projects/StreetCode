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
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class ExtractUpdateTeamMemberAttribute : BeforeAfterTestAttribute
    {
        private TeamMember _teamMember = null!;
        private Image _image = null!;

        public static UpdateTeamMemberDTO TeamMemberForTest { get; set; } = null!;

        public override void Before(MethodInfo methodUnderTest)
        {
            int uniqueId = UniqueNumberGenerator.GenerateInt();
            _image = ImageExtracter.Extract(uniqueId);
            _teamMember = TeamMemberExtracter.Extract(uniqueId, _image.Id);

            TeamMemberForTest = new UpdateTeamMemberDTO
            {
                Id = _teamMember.Id,
                Name = "test",
                Description = "test description",
                IsMain = false,
                ImageId = _image.Id,
            };
        }

        public override void After(MethodInfo methodUnderTest)
        {
            var sqlDbHelper = BaseControllerTests.GetSqlDbHelper();
            var teamMember = sqlDbHelper.GetExistItem<TeamMember>(t => t.Id == TeamMemberForTest.Id);
            if (teamMember != null)
            {
                sqlDbHelper.DeleteItem(teamMember);
                sqlDbHelper.SaveChanges();
            }

            var image = sqlDbHelper.GetExistItem<Image>(t => t.Id == _image.Id);
            if (image != null)
            {
                sqlDbHelper.DeleteItem(image);
                sqlDbHelper.SaveChanges();
            }
        }
    }
}
