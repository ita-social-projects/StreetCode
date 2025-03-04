using Streetcode.DAL.Entities.Team;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.Team
{
    public static class TeamLinkExtracter
    {
        public static TeamMemberLink Extract(int linkId, int teamMemberId)
        {
            TeamMemberLink testTeamLink = TestDataProvider.GetTestData<TeamMemberLink>();

            testTeamLink.Id = linkId;
            testTeamLink.TeamMemberId = teamMemberId;

            return BaseExtracter.Extract<TeamMemberLink>(testTeamLink, teamLink => teamLink.Id == linkId);
        }

        public static void Remove(TeamMemberLink entity)
        {
            BaseExtracter.RemoveByPredicate<TeamMemberLink>(teamLink => teamLink.Id == entity.Id);
        }
    }
}
