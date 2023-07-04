using Streetcode.DAL.Entities.AdditionalContent.Coordinates.Types;
using Streetcode.DAL.Entities.Transactions;
using Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Streetcode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Xunit.Sdk;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Transactions
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    internal class ExtractTestTransactionLink : BeforeAfterTestAttribute
    {
        public static TransactionLink TransactionLinkForTest;

        public override void Before(MethodInfo methodUnderTest)
        {
            var sqlDbHelper = BaseControllerTests.GetSqlDbHelper();
            TransactionLinkForTest = sqlDbHelper.GetExistItem<TransactionLink>();

            if (TransactionLinkForTest == null)
            {
                new ExtractTestStreetcode().Before(methodUnderTest);
                TransactionLinkForTest = sqlDbHelper.AddNewItem(new TransactionLink()
                {
                    Url = "url",
                    UrlTitle = "url title",
                    StreetcodeId = ExtractTestStreetcode.StreetcodeForTest.Id
                });
                sqlDbHelper.SaveChanges();
            }
        }
    }
}
