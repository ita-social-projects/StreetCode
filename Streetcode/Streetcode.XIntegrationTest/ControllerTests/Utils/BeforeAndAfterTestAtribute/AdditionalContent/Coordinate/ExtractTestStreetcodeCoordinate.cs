using Streetcode.DAL.Entities.AdditionalContent.Coordinates.Types;
using Streetcode.DAL.Entities.Streetcode;
using System.Reflection;
using Xunit.Sdk;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.AdditionalContent.Coordinate
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class ExtractTestStreetcodeCoordinate : BeforeAfterTestAttribute
    {
        public static StreetcodeCoordinate CoordinateForTest;

        public override void Before(MethodInfo methodUnderTest)
        {
            var sqlDbHelper = BaseControllerTests.GetSqlDbHelper();
            CoordinateForTest = sqlDbHelper.GetExistItem<StreetcodeCoordinate>();

            if (CoordinateForTest == null)
            {
                StreetcodeContent first = sqlDbHelper.GetExistItem<StreetcodeContent>();
                first ??= sqlDbHelper.AddNewItem(new StreetcodeContent()
                {
                    Index = 10,
                    UpdatedAt = DateTime.Now,
                    CreatedAt = DateTime.Now,
                    EventStartOrPersonBirthDate = DateTime.Now,
                    EventEndOrPersonDeathDate = DateTime.Now,
                    ViewCount = 1,
                });

                CoordinateForTest = sqlDbHelper.AddNewItem(
                    new StreetcodeCoordinate()
                    {
                        Latitude = 44,
                        Longtitude = 50,
                        StreetcodeId = first.Id,
                    });
                sqlDbHelper.SaveChanges();
            }
        }
    }
}
