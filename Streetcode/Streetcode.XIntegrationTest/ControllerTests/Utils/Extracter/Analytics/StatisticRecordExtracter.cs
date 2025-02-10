using Streetcode.DAL.Entities.AdditionalContent.Coordinates.Types;
using Streetcode.DAL.Entities.Analytics;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.Analytics;

public static class StatisticRecordExtracter
{
    public static StatisticRecord Extract(int id, int streetcodeId)
    {
        StatisticRecord testStatisticRecord = TestDataProvider.GetTestData<StatisticRecord>();
        StreetcodeCoordinate testStreetcodeCoordinate = new StreetcodeCoordinate
        {
            StreetcodeId = streetcodeId,
            Latitude = 50,
            Longtitude = 50,
        };
        testStatisticRecord.StreetcodeCoordinate = testStreetcodeCoordinate;
        testStatisticRecord.StreetcodeId = streetcodeId;
        testStatisticRecord.Id = id;

        return BaseExtracter.Extract(testStatisticRecord, strCont => strCont.Id == id);
    }

    public static void Remove(StatisticRecord entity)
    {
        BaseExtracter.RemoveByPredicate<StatisticRecord>(strCont => strCont.Id == entity.Id);
    }
}