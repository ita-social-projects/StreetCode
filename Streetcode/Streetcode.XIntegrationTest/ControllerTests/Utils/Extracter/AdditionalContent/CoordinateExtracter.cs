using Streetcode.DAL.Entities.AdditionalContent.Coordinates.Types;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.AdditionalContent
{
    public static class CoordinateExtracter
    {
        public static StreetcodeCoordinate Extract(int streetcodeCoordinateId, int streetcodeId)
        {
            StreetcodeCoordinate testCoordinate = TestDataProvider.GetTestData<StreetcodeCoordinate>();

            testCoordinate.Id = streetcodeCoordinateId;
            testCoordinate.StreetcodeId = streetcodeId;

            return BaseExtracter.Extract(testCoordinate, coordinate => coordinate.Id == streetcodeId);
        }

        public static void Remove(StreetcodeCoordinate entity)
        {
            BaseExtracter.RemoveByPredicate<StreetcodeCoordinate>(coordinate => coordinate.Id == entity.Id);
        }
    }
}
