using Streetcode.DAL.Entities.Toponyms;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.Toponyms
{
    public static class ToponymExtracter
    {
        public static Toponym Extract(int toponymId)
        {
            var toponym = TestDataProvider.GetTestData<Toponym>();
            toponym.Id = toponymId;

            return BaseExtracter.Extract<Toponym>(toponym, t => t.Id == toponymId);
        }

        public static void Remove(Toponym toponym)
        {
            BaseExtracter.RemoveByPredicate<Toponym>(t => t.Id == toponym.Id);
        }

        public static void AddStreetcodeToponym(int toponymId, int streetcodeId)
        {
            var streetcodeToponym = new StreetcodeToponym()
            {
                ToponymId = toponymId,
                StreetcodeId = streetcodeId,
            };
            BaseExtracter.Extract<StreetcodeToponym>(streetcodeToponym, t => t.ToponymId == toponymId, hasIdentity: false);
        }
    }
}
