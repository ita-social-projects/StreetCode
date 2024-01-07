using Streetcode.DAL.Entities.AdditionalContent;
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
                });
                sqlDbHelper.SaveChanges();
            }
        }
    }
}
