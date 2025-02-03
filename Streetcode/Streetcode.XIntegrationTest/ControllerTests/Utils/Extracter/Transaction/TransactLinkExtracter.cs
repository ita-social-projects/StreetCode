using Streetcode.DAL.Entities.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.Transaction
{
    public static class TransactLinkExtracter
    {
        public static TransactionLink Extract(int transactId)
        {
            TransactionLink transactionLink = TestDataProvider.GetTestData<TransactionLink>();
            transactionLink.Id = transactId;

            return BaseExtracter.Extract<TransactionLink>(transactionLink, transact => transact.Id == transactId);
        }

        public static void Remove(TransactionLink transact)
        {
            BaseExtracter.RemoveByPredicate<TransactionLink>(t => t.Id == transact.Id);
        }
    }
}
