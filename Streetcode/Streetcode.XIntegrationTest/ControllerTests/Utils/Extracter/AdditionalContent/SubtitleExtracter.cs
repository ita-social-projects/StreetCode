using Streetcode.DAL.Entities.AdditionalContent;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.StreetcodeExtracter;
using Streetcode.XIntegrationTest.ServiceTests.BlobServiceTests.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            return BaseExtracter.Extract<Subtitle>(testSubtitle, image => image.Id == subtitleId);
        }

        public static void Remove(Subtitle entity)
        {
            BaseExtracter.RemoveByPredicate<Subtitle>(image => image.Id == entity.Id);
            BaseExtracter.RemoveById<StreetcodeContent>(entity.Id);
        }
    }
}
