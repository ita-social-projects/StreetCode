using Streetcode.DAL.Entities.Streetcode.TextContent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Streetcode.TextContent.Facts
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class ExtractTestFact: BeforeAfterTestAttribute
    {
        public static Fact FactForTest;

        public override void Before(MethodInfo methodUnderTest)
        {
            var sqlDbHelper = BaseControllerTests.GetSqlDbHelper();
            FactForTest = sqlDbHelper.GetExistItem<Fact>();
            if (FactForTest == null)
            {
                new ExtractTestFact().Before(null);
                FactForTest = sqlDbHelper.AddNewItem(new Fact()
                {
                    Title = "FactForTest title ",
                    FactContent = "factcontent",
                    ImageId = ExtractTestFact.FactForTest.Id,
                });
                sqlDbHelper.SaveChanges();
            }
        }
    }
}
