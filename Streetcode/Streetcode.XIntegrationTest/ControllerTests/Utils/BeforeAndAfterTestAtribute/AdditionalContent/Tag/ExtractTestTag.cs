using System.Reflection;
using Xunit.Sdk;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.AdditionalContent.Tag
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class ExtractTestTag : BeforeAfterTestAttribute
    {
        public static DAL.Entities.AdditionalContent.Tag TagForTest;

        public override void Before(MethodInfo methodUnderTest)
        {
            var sqlDbHelper = BaseControllerTests.GetSqlDbHelper();
            TagForTest = sqlDbHelper.GetExistItem<DAL.Entities.AdditionalContent.Tag>();
            if (TagForTest == null)
            {
                TagForTest = sqlDbHelper.AddNewItem(
                    new DAL.Entities.AdditionalContent.Tag()
                    {
                        Title = "TagTitle_NEW!!!",
                    });
                sqlDbHelper.SaveChanges();
            }
        }
    }
}
