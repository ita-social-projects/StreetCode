using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Entities.Transactions;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.StreetcodeExtracter;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.Transaction
{
    public static class TransactLinkExtracter
    {
        public static TransactionLink Extract(int transactId)
        {
            TransactionLink transactionLink = TestDataProvider.GetTestData<TransactionLink>();

            StreetcodeContent testStreetcodeContent = StreetcodeContentExtracter.Extract(
                transactId,
                transactId,
                Guid.NewGuid().ToString());

            transactionLink.StreetcodeId = testStreetcodeContent.Id;
            transactionLink.Id = transactId;

            return BaseExtracter.Extract<TransactionLink>(transactionLink, transact => transact.Id == transactId);
        }

        public static void Remove(TransactionLink transact)
        {
            BaseExtracter.RemoveByPredicate<TransactionLink>(t => t.Id == transact.Id);
        }
    }
}
