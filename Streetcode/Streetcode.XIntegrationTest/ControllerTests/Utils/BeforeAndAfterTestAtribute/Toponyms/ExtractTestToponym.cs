using Streetcode.DAL.Entities.Toponyms;
using Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.AdditionalContent.Coordinate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit.Sdk;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Toponyms
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    internal class ExtractTestToponym:BeforeAfterTestAttribute
    {
        public static Toponym ToponymForTest;

        public override void Before(MethodInfo methodUnderTest)
        {
            var sqlDbHelper = BaseControllerTests.GetSqlDbHelper();
            ToponymForTest = sqlDbHelper.GetExistItem<Toponym>();

            if (ToponymForTest == null)
            {
                new ExtractTestStreetcodeCoordinate().Before(null);
                ToponymForTest = sqlDbHelper.AddNewItem(new Toponym()
                {
                    AdminRegionNew = "new admin region",
                    AdminRegionOld = "old adm region",
                    Community = "comunity",
                    Gromada = "gromadaname",
                    Oblast = "Lviv",
                    StreetName = "Independence Street",
                });
                sqlDbHelper.SaveChanges();
            }
        }
    }
}
