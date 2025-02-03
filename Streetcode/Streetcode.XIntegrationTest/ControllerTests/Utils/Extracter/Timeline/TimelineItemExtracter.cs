using Streetcode.DAL.Entities.AdditionalContent;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Entities.Timeline;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.StreetcodeExtracter;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.Timeline
{
    public static class TimelineItemExtracter
    {
        public static TimelineItem Extract(int timelineId)
        {
            TimelineItem item = TestDataProvider.GetTestData<TimelineItem>();
            StreetcodeContent testStreetcodeContent = StreetcodeContentExtracter.Extract(
                timelineId,
                timelineId,
                Guid.NewGuid().ToString());
            item.StreetcodeId = testStreetcodeContent.Id;
            item.Id = timelineId;

            return BaseExtracter.Extract<TimelineItem>(item, t => t.Id == timelineId);
        }

        public static void Remove(TimelineItem item)
        {
            BaseExtracter.RemoveByPredicate<TimelineItem>(t => t.Id == item.Id);
        }
    }
}
