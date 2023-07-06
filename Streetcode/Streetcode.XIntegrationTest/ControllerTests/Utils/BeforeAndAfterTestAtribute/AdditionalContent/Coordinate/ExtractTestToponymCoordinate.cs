using Streetcode.DAL.Entities.AdditionalContent.Coordinates.Types;
using Streetcode.DAL.Entities.Toponyms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.AdditionalContent.Coordinate
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    internal class ExtractTestToponymCoordinate:BeforeAfterTestAttribute
    {
        public static ToponymCoordinate ToponymCoordinateForTest;

        public override void Before(MethodInfo methodUnderTest)
        {
            var sqlDbHelper = BaseControllerTests.GetSqlDbHelper();
            ToponymCoordinateForTest = sqlDbHelper.GetExistItem<ToponymCoordinate>();

            if (ToponymCoordinateForTest == null)
            {
                var toponym = sqlDbHelper.GetExistItem<Toponym>();
                if (toponym == null)
                {
                    toponym = sqlDbHelper.AddNewItem(new Toponym()
                    {
                        AdminRegionNew = "new region",
                        AdminRegionOld = "old",
                        Community = "community",
                        Gromada = "gromada string",
                        Oblast = "obl",
                        StreetName = "street name",
                    });
                }

                sqlDbHelper.SaveChanges();
                ToponymCoordinateForTest = sqlDbHelper.AddNewItem(new ToponymCoordinate()
                {
                    Latitude = 40,
                    Longtitude = 50,
                    ToponymId = toponym.Id,
                });
                sqlDbHelper.SaveChanges();
            }
        }
    }
}
