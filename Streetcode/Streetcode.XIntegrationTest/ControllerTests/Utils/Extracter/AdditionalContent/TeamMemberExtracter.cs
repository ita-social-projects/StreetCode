using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Entities.Team;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.MediaExtracter.Image;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.AdditionalContent
{
    public static class TeamMemberExtracter
    {
        public static TeamMember Extract(int teamMemberId, int imageId)
        {
            TeamMember testTeamMember = TestDataProvider.GetTestData<TeamMember>();

            testTeamMember.Id = teamMemberId;
            testTeamMember.ImageId = imageId;

            ImageExtracter.Extract(imageId);
            return BaseExtracter.Extract<TeamMember>(testTeamMember, teamMember => teamMember.Id == teamMemberId);
        }

        public static void Remove(TeamMember entity)
        {
            BaseExtracter.RemoveByPredicate<TeamMember>(teamMember => teamMember.Id == entity.Id);
            BaseExtracter.RemoveByPredicate<Image>(image => image.Id == entity.ImageId);
        }
    }
}
