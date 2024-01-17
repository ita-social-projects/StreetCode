using Streetcode.DAL.Entities.Sources;
using Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Media.Images.Image;
using System.Reflection;
using Xunit.Sdk;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Source.SourceLinkCategories
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    internal class ExtractTestSourceLinkCategory : BeforeAfterTestAttribute
    {
        public static SourceLinkCategory SourceLinkCategoryForTest;

        public override void Before(MethodInfo methodUnderTest)
        {
            var sqlDbHelper = BaseControllerTests.GetSqlDbHelper();
            SourceLinkCategoryForTest = sqlDbHelper.GetExistItem<SourceLinkCategory>();
            if (SourceLinkCategoryForTest == null)
            {
                new ExtractTestImage().Before(null);
                SourceLinkCategoryForTest = sqlDbHelper.AddNewItem(
                    new SourceLinkCategory()
                    {
                        Title = "SourceLinkCategory title",
                        ImageId = ExtractTestImage.ImageForTest.Id,

                    });
                sqlDbHelper.SaveChanges();
            }
        }
    }
}
