using Streetcode.DAL.Entities.AdditionalContent;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.StreetcodeExtracter;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.AdditionalContent
{
    public class SubtitleExtracter
    {
        public static Subtitle Extract(int subtitleId)
        {
            Subtitle testSubtitle = TestDataProvider.GetTestData<Subtitle>();
            StreetcodeContent testStreetcodeContent = StreetcodeContentExtracter.Extract(
                subtitleId,
                subtitleId,
                Guid.NewGuid().ToString());
            testSubtitle.Id = subtitleId;
            testSubtitle.StreetcodeId = testStreetcodeContent.Id;

            return BaseExtracter.Extract<Subtitle>(testSubtitle, subtitle => subtitle.Id == subtitleId);
        }

        public static void Remove(Subtitle entity)
        {
            BaseExtracter.RemoveByPredicate<Subtitle>(subtitle => subtitle.Id == entity.Id);
            BaseExtracter.RemoveById<StreetcodeContent>(entity.Id);
        }
    }
}
