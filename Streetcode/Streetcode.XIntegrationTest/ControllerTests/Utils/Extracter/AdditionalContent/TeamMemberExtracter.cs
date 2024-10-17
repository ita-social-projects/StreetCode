using Streetcode.DAL.Entities.Team;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.AdditionalContent
{
    public static class TeamMemberExtracter
    {
        public static TeamMember Extract(int teamMemberId)
        {
            TeamMember testTeamMember = TestDataProvider.GetTestData<TeamMember>();

            testTeamMember.Id = teamMemberId;

            return BaseExtracter.Extract<TeamMember>(testTeamMember, teamMember => teamMember.Id == teamMemberId);
        }

        public static void Remove(TeamMember entity)
        {
            BaseExtracter.RemoveByPredicate<TeamMember>(teamMember => teamMember.Id == entity.Id);
        }
    }
}
