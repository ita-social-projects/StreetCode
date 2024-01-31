using Streetcode.DAL.Entities.Streetcode;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.StreetcodeExtracter
{
    public static class StreetcodeContentExtracter
    {
        public static StreetcodeContent Extract(int id, int index, string transliterationUrl)
        {
            StreetcodeContent testStreetcodeContent = TestDataProvider.GetTestData<StreetcodeContent>();
            testStreetcodeContent.Id = id;
            testStreetcodeContent.Index = index;
            testStreetcodeContent.TransliterationUrl = transliterationUrl;

            return BaseExtracter.Extract<StreetcodeContent>(testStreetcodeContent, strCont => strCont.Id == id);
        }

        public static void Remove(StreetcodeContent entity)
        {
            BaseExtracter.RemoveByPredicate<StreetcodeContent>(strCont => strCont.Id == entity.Id);
        }
    }
}
