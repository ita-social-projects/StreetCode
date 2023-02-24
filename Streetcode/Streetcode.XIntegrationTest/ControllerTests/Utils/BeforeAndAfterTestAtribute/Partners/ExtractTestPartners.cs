using Streetcode.DAL.Entities.Partners;
using Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Media.Images.Image;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Partners
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    internal class ExtractTestPartners: BeforeAfterTestAttribute
    {
        public static Partner PartnerForTest;

        public override void Before(MethodInfo methodUnderTest)
        {
            var sqlDbHelper = BaseControllerTests.GetSqlDbHelper();
            PartnerForTest = sqlDbHelper.GetExistItem<Partner>();
            if (PartnerForTest == null)
            {
                new ExtractTestImage().Before(null);
                PartnerForTest = sqlDbHelper.AddNewItem(new Partner()
                {
                    Description = "PartnerDescription",
                    IsKeyPartner = true,
                    TargetUrl = "PartnerTargetUrl",
                    Title = "PartnerTitle",
                    UrlTitle = "PartnerUrl",
                    LogoId = ExtractTestImage.ImageForTest.Id,
                });
                sqlDbHelper.SaveChanges();
            }
        }
    }
}
