using Streetcode.DAL.Entities.Team;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.AdditionalContent;

public static class TeamPositionsExtracter
{
    public static Positions Extract(int positionId)
    {
        Positions testPosition = TestDataProvider.GetTestData<Positions>();

        testPosition.Id = positionId;

        return BaseExtracter.Extract<Positions>(testPosition, position => position.Id == positionId);
    }

    public static void Remove(Positions entity)
    {
        BaseExtracter.RemoveByPredicate<Positions>(position => position.Id == entity.Id);
    }

    public static void AddTeamMemberPositions(int teamMemberId, int positionsId)
    {
        TeamMemberPositions teamMemberPositions = new TeamMemberPositions()
        {
            TeamMemberId = teamMemberId,
            PositionsId = positionsId,
        };
        BaseExtracter.Extract(
            teamMemberPositions,
            strArt => strArt.TeamMemberId == teamMemberId && strArt.PositionsId == positionsId,
            false);
    }
}
