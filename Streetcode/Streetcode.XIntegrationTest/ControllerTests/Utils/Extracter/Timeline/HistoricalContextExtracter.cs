using Streetcode.DAL.Entities.Timeline;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.Timeline;

public class HistoricalContextExtracter
{
    public static HistoricalContext Extract(int contextId)
    {
        HistoricalContext testContext = TestDataProvider.GetTestData<HistoricalContext>();

        testContext.Id = contextId;

        return BaseExtracter.Extract<HistoricalContext>(testContext, context => context.Id == contextId);
    }

    public static void Remove(HistoricalContext entity)
    {
        BaseExtracter.RemoveByPredicate<HistoricalContext>(context => context.Id == entity.Id);
    }

    public static void AddHistoricalContextTimeline(int timeLineId, int historicalContextId)
    {
        var historicalContextTimeline = new HistoricalContextTimeline()
        {
            HistoricalContextId = historicalContextId,
            TimelineId = timeLineId,
        };
        BaseExtracter.Extract(historicalContextTimeline, context => context.TimelineId == timeLineId, hasIdentity: false);
    }
}