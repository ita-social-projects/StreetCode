using Streetcode.DAL.Entities.Streetcode;
using Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Streetcode;
using System.Reflection;
using Xunit.Sdk;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.AdditionalContent.Subtitle
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class ExtractTestSubtitle : BeforeAfterTestAttribute
    {
        public static DAL.Entities.AdditionalContent.Subtitle SubtitleForTest;

        public override void Before(MethodInfo methodUnderTest)
        {
            var sqlDbHelper = BaseControllerTests.GetSqlDbHelper();
            SubtitleForTest = sqlDbHelper.GetExistItem<DAL.Entities.AdditionalContent.Subtitle>();
            var streetCode = sqlDbHelper.GetExistItem<StreetcodeContent>();
            int streetcodeId;
            if (streetCode == null)
            {
                new ExtractTestStreetcode().Before(methodUnderTest);
                streetcodeId = ExtractTestStreetcode.StreetcodeForTest.Id;
            }
            else
            {
                streetcodeId = streetCode.Id;
            }

            if (SubtitleForTest == null)
            {
                SubtitleForTest = sqlDbHelper.AddNewItem(new DAL.Entities.AdditionalContent.Subtitle()
                {
                    SubtitleText = "text",
                    StreetcodeId = streetcodeId,
                });
            }

            sqlDbHelper.SaveChanges();


        }
    }
}
