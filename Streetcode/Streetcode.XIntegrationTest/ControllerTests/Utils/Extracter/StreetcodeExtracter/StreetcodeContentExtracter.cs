using Streetcode.DAL.Entities.Media;
using Streetcode.DAL.Entities.Streetcode;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.StreetcodeExtracter
{
    public static class StreetcodeContentExtracter
    {
        public static StreetcodeContent Extract(int id, int index, string transliterationUrl, Audio? audio = null, DateTime? createdAt = null)
        {
            StreetcodeContent testStreetcodeContent = TestDataProvider.GetTestData<StreetcodeContent>();
            testStreetcodeContent.Id = id;
            testStreetcodeContent.Index = index;
            testStreetcodeContent.TransliterationUrl = transliterationUrl;
            if (audio is not null)
            {
                testStreetcodeContent.Audio = audio;
                testStreetcodeContent.AudioId = audio.Id;
            }

            if (createdAt is null)
            {
                testStreetcodeContent.CreatedAt = DateTime.Now;
            }
            else
            {
                testStreetcodeContent.CreatedAt = (DateTime)createdAt;
            }

            return BaseExtracter.Extract<StreetcodeContent>(testStreetcodeContent, strCont => strCont.Id == id);
        }

        public static void Remove(StreetcodeContent entity)
        {
            BaseExtracter.RemoveByPredicate<StreetcodeContent>(strCont => strCont.Id == entity.Id);
        }
    }
}
