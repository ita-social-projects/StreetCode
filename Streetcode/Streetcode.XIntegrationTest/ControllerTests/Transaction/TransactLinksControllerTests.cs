using System.Net;
using Streetcode.BLL.DTO.Transactions;
using Streetcode.DAL.Entities.Transactions;
using Streetcode.XIntegrationTest.Base;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Transaction;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.Transaction;
using Xunit;

namespace Streetcode.XIntegrationTest.ControllerTests.Transaction
{
    [Collection("Transaction")]
    public class TransactLinksControllerTests : BaseControllerTests<TransactLinksClient>, IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly TransactionLink transactionLink;

        public TransactLinksControllerTests(CustomWebApplicationFactory<Program> factory)
            : base(factory, "api/TransactLinks")
        {
            int uniqueId = UniqueNumberGenerator.GenerateInt();
            this.transactionLink = TransactLinkExtracter.Extract(uniqueId);
        }

        [Fact]
        public async Task GetAll_ReturnsSuccessStatusCode()
        {
            // Act
            var response = await this.Client.GetAllAsync();
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<IEnumerable<TransactLinkDTO>>(response.Content);

            // Assert
            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(returnedValue);
        }

        [Fact]
        public async Task GetByStreetcodeId_ReturnsSuccessStatusCode()
        {
            // Arrange
            int streetcodeId = this.transactionLink.StreetcodeId;

            // Act
            var response = await this.Client.GetByStreetcodeId(streetcodeId);
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<TransactLinkDTO>(response.Content);

            // Assert
            Assert.True(response.IsSuccessful);
            Assert.NotNull(returnedValue);
            Assert.Equal(streetcodeId, returnedValue.StreetcodeId);
        }

        [Fact]
        public async Task GetByStreetcodeIdIncorrect_ReturnsBadRequest()
        {
            // Arrange
            int incorrectStreetcodeId = -1;

            // Act
            var response = await this.Client.GetByStreetcodeId(incorrectStreetcodeId);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.False(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task GetById_ReturnsSuccessStatusCode()
        {
            // Arrange
            TransactionLink expectedTransactLink = this.transactionLink;

            // Act
            var response = await this.Client.GetByIdAsync(expectedTransactLink.Id);
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<TransactLinkDTO>(response.Content);

            // Assert
            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(returnedValue);
            Assert.Equal(expectedTransactLink.Id, returnedValue.Id);
            Assert.Equal(expectedTransactLink.Url, returnedValue.Url);
        }

        [Fact]
        public async Task GetByIdIncorrect_ReturnsBadRequest()
        {
            // Arrange
            int incorrectId = -1;

            // Act
            var response = await this.Client.GetByIdAsync(incorrectId);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.False(response.IsSuccessStatusCode);
        }

        
    }
}
