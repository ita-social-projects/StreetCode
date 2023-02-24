using Microsoft.EntityFrameworkCore.Diagnostics;
using Streetcode.DAL.Entities.Streetcode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit.Sdk;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Streetcode
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    class ExtractTestStreetcode:BeforeAfterTestAttribute
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
                    Index = 10,
                    UpdatedAt = DateTime.Now,
                    CreatedAt = DateTime.Now,
                    EventStartOrPersonBirthDate = DateTime.Now,
                    EventEndOrPersonDeathDate = DateTime.Now,
                    ViewCount = 1,
                });
                sqlDbHelper.SaveChanges();
            }
        }
    }
}
