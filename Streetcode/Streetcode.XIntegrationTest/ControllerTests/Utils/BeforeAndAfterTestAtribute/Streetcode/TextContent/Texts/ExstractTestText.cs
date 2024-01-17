using Streetcode.DAL.Entities.Streetcode.TextContent;
using System.Reflection;
using Xunit.Sdk;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Streetcode.TextContent.Texts
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    internal class ExstractTestText : BeforeAfterTestAttribute
    {
        public static Text TextForTest;

        public override void Before(MethodInfo methodUnderTest)
        {
            var sqlDbHelper = BaseControllerTests.GetSqlDbHelper();
            TextForTest = sqlDbHelper.GetExistItem<Text>();

            if (TextForTest == null)
            {
                new ExtractTestStreetcode().Before(methodUnderTest);
                TextForTest = sqlDbHelper.AddNewItem(new Text()
                {
                    TextContent = "Text content",
                    Title = "Text title",
                    StreetcodeId = ExtractTestStreetcode.StreetcodeForTest.Id
                });
            }
        }
    }
}
