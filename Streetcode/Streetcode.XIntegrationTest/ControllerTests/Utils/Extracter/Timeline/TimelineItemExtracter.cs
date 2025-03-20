using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Entities.Timeline;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.StreetcodeExtracter;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.Timeline
{
    public static class TimelineItemExtracter
    {
        public static TimelineItem Extract(int timelineId, int streetCodeId)
        {
            TimelineItem item = TestDataProvider.GetTestData<TimelineItem>();
            item.StreetcodeId = streetCodeId;
            item.Id = timelineId;

            return BaseExtracter.Extract(item, t => t.Id == timelineId);
        }

        public static void Remove(TimelineItem item)
        {
            BaseExtracter.RemoveByPredicate<TimelineItem>(t => t.Id == item.Id);
        }
    }
}
