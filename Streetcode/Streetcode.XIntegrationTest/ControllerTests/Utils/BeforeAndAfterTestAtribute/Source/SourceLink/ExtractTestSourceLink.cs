using Streetcode.DAL.Entities.Sources;
using System.Reflection;
using Xunit.Sdk;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Source.SourceLink
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    internal class ExtractTestSourceLink:BeforeAfterTestAttribute
    {
        public static DAL.Entities.Sources.SourceLink SourceLinkForTest;
        public override void Before(MethodInfo methodUnderTest)
        {
            var sqlDbHelper = BaseControllerTests.GetSqlDbHelper();
            SourceLinkForTest = sqlDbHelper.GetExistItem<DAL.Entities.Sources.SourceLink>();
            if (SourceLinkForTest == null)
            {
                SourceLinkForTest = sqlDbHelper.AddNewItem( new DAL.Entities.Sources.SourceLink()
                {
                    Title = "SourceLink title ",
                    Url = "SourceLink url",
                } );
                sqlDbHelper.SaveChanges();
            }
        }
    }
}
