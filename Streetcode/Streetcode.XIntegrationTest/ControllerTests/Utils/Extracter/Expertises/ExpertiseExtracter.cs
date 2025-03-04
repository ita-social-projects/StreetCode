using Streetcode.DAL.Entities.Users.Expertise;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.Expertises;

public static class ExpertiseExtracter
{
    public static Expertise Extract(int expertiseId)
    {
        var testExpertise = TestDataProvider.GetTestData<Expertise>();

        testExpertise.Id = expertiseId;
        var extracter = BaseExtracter.Extract(testExpertise, e => e.Id == expertiseId);

        return extracter;
    }
}