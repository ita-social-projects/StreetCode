using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Streetcode.DAL.Entities;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Persistence;
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
                streetcodeId = sqlDbHelper.AddNewItem<StreetcodeContent>(new StreetcodeContent()
                {
                    CreatedAt = DateTime.Now,
                    Index = 0,
                    ViewCount = 0
                }).Id;
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
