using Streetcode.DAL.Entities.Users.Expertise;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.Expertises;

public static class ExpertiseExtracter
{
    public static Expertise Extract(int expertiseId)
    {
        var testExpertise = TestDataProvider.GetTestData<Expertise>();

        testExpertise.Id = expertiseId;
        var extracter = BaseExtracter.Extract<Expertise>(testExpertise, e => e.Id == expertiseId);
        return extracter;
    }

    public static void Remove(Expertise entity)
    {
        BaseExtracter.RemoveByPredicate<Expertise>(e => e.Id == entity.Id);
    }
}