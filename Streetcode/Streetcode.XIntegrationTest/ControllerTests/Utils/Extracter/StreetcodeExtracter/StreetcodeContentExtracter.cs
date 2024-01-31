using Streetcode.DAL.Entities.Streetcode;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.StreetcodeExtracter
{
    public static class StreetcodeContentExtracter
    {
        public static StreetcodeContent Extract(int index, string transliterationUrl)
        {
            StreetcodeContent testStreetcodeContent = TestDataProvider.GetTestData<StreetcodeContent>();
            testStreetcodeContent.Index = index;
            testStreetcodeContent.TransliterationUrl = transliterationUrl;

            return BaseExtracter.Extract<StreetcodeContent>(testStreetcodeContent, strCont => strCont.Index == index);
        }

        public static void Remove(StreetcodeContent streetcodeContent)
        {
            BaseExtracter.Remove<StreetcodeContent>(streetcodeContent, strCont => strCont.Index == streetcodeContent.Index);
        }
    }
}
