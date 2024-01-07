using Streetcode.DAL.Entities.Streetcode;
using System.Reflection;
using Xunit.Sdk;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Streetcode
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    class ExtractTestStreetcode : BeforeAfterTestAttribute
    {
        public static StreetcodeContent StreetcodeForTest;

        public override void Before(MethodInfo methodUnderTest)
        {
            var sqlDbHelper = BaseControllerTests.GetSqlDbHelper();
            StreetcodeForTest = sqlDbHelper.GetExistItem<StreetcodeContent>();
            if (StreetcodeForTest == null)
            {
                StreetcodeForTest = sqlDbHelper.AddNewItem(new StreetcodeContent()
                {
                    Index = new Random().Next(0, 1000000),
                    UpdatedAt = DateTime.Now,
                    CreatedAt = DateTime.Now,
                    EventStartOrPersonBirthDate = DateTime.Now,
                    EventEndOrPersonDeathDate = DateTime.Now,
                    ViewCount = 1,
                    DateString = "20 травня 2023",
                    Alias = "dsf",
                    Title = "Title",
                    TransliterationUrl = Guid.NewGuid().ToString(),
                    Teaser = "Test Teaser",
                });
                sqlDbHelper.SaveChanges();
            }
        }

        public override void After(MethodInfo methodUnderTest)
        {
            var sqlDbHelper = BaseControllerTests.GetSqlDbHelper();
            var streetcodeContent = sqlDbHelper.GetExistItem<StreetcodeContent>();
            if (streetcodeContent != null)
            {
                // Restore the original StreetcodeContent
                streetcodeContent.EventStartOrPersonBirthDate = StreetcodeForTest.EventStartOrPersonBirthDate;
                streetcodeContent.EventEndOrPersonDeathDate = StreetcodeForTest.EventEndOrPersonDeathDate;
                streetcodeContent.ViewCount = StreetcodeForTest.ViewCount;
                streetcodeContent.DateString = StreetcodeForTest.DateString;
                streetcodeContent.Alias = StreetcodeForTest.Alias;
                streetcodeContent.Title = StreetcodeForTest.Title;
                streetcodeContent.TransliterationUrl = StreetcodeForTest.TransliterationUrl;
                streetcodeContent.Teaser = StreetcodeForTest.Teaser;

                sqlDbHelper.SaveChanges();
            }
        }
    }
}
