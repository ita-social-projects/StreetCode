using Streetcode.DAL.Entities.Team;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.AdditionalContent;

public class TeamPositionsExtracter
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
}
