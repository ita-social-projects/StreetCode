using Streetcode.DAL.Entities.Streetcode;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.StreetcodeExtracter
{
    public class StreetcodeContentExtracter
    {
        private static StreetcodeContent? _streetcodeContent;

        public static StreetcodeContent Extract(int index, string transliterationUrl)
        {
            if (_streetcodeContent is not null)
            {
                return _streetcodeContent;
            }

            StreetcodeContent testStreetcodeContent = TestDataProvider.GetTestData<StreetcodeContent>();
            testStreetcodeContent.Index = index;
            testStreetcodeContent.TransliterationUrl = transliterationUrl;

            return BaseExtracter.Extract<StreetcodeContent>(testStreetcodeContent, strCont => strCont.Index == index);
        }
    }
}
