using Streetcode.DAL.Entities.Toponyms;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.Toponyms
{
    public static class ToponymExtracter
    {
        public static Toponym Extract(int toponymId)
        {
            Toponym toponym = TestDataProvider.GetTestData<Toponym>();
            toponym.Id = toponymId;

            return BaseExtracter.Extract<Toponym>(toponym, t => t.Id == toponymId);
        }

        public static void Remove(Toponym toponym)
        {
            BaseExtracter.RemoveByPredicate<Toponym>(t => t.Id == toponym.Id);
        }
    }
}
