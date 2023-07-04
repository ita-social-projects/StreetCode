﻿using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Streetcode.TextContent.Facts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Streetcode.TextContent.Terms
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    internal class ExstractTestTerm: BeforeAfterTestAttribute
    {
        public static Term TermForTest;

        public override void Before(MethodInfo methodUnderTest)
        {
            var sqlDbHelper = BaseControllerTests.GetSqlDbHelper();
            TermForTest = sqlDbHelper.GetExistItem<Term>();
            if (TermForTest == null)
            {
                TermForTest = sqlDbHelper.AddNewItem(new Term()
                {
                    Title = "Term title ",
                    Description = "Term description",
                });
                sqlDbHelper.SaveChanges();
            }
        }
    }
}
