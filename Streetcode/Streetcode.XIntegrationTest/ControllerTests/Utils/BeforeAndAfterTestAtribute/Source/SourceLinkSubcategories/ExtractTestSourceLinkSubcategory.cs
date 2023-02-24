using Streetcode.DAL.Entities.Sources;
using Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Source.SourceLinkCategories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Source.SourceLinkSubcategories
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    internal class ExtractTestSourceLinkSubcategory:BeforeAfterTestAttribute
    {
        public static SourceLinkSubCategory SourceLinkSubCategoryForTest;

        public override void Before(MethodInfo methodUnderTest)
        {
            var sqlDbHelper = BaseControllerTests.GetSqlDbHelper();
            SourceLinkSubCategoryForTest = sqlDbHelper.GetExistItem<SourceLinkSubCategory>();
            if (SourceLinkSubCategoryForTest == null)
            {
                new ExtractTestSourceLinkCategory().Before(null);

                SourceLinkSubCategoryForTest = sqlDbHelper.AddNewItem(
                    new SourceLinkSubCategory()
                    {
                        Title = "SourceLinkSubCategory title",
                        SourceLinkCategoryId = ExtractTestSourceLinkCategory.SourceLinkCategoryForTest.Id,
                    });
                sqlDbHelper.SaveChanges();
            }
        }
    }
}
