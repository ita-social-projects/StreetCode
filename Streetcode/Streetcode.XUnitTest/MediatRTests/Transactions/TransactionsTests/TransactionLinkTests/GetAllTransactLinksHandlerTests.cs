using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Transactions;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Transactions.TransactionLink.GetAll;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Transactions;
using Streetcode.DAL.Repositories.Interfaces.Base;

using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Transactions.TransactionsTests.TransactionLinkTests
{
    public class GetAllTransactLinksHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> mockRepo;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<ILoggerService> mockLogger;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> mockLocalizerCannotFind;

        private readonly List<TransactionLink> transactions = new List<TransactionLink>()
        {
            new TransactionLink
            {
                Id = 1,
                Url = "URL",
                UrlTitle = "Title",
                StreetcodeId = 1,
            },
            new TransactionLink
            {
                Id = 2,
                Url = "URL2",
                UrlTitle = "Title2",
                StreetcodeId = 2,
            },
        };

        private readonly List<TransactLinkDto> transactionsDTOs = new List<TransactLinkDto>()
        {
            new TransactLinkDto
            {
                Id = 1,
                Url = "URL",
                QrCodeUrl = "URL",
                StreetcodeId = 1,
            },
            new TransactLinkDto
            {
                Id = 2,
                Url = "URL2",
                QrCodeUrl = "URL2",
                StreetcodeId = 2,
            },
        };

        public GetAllTransactLinksHandlerTests()
        {
            this.mockMapper = new Mock<IMapper>();
            this.mockRepo = new Mock<IRepositoryWrapper>();
            this.mockLogger = new Mock<ILoggerService>();
            this.mockLocalizerCannotFind = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        }

        [Fact]
        public async Task NotEmpty_List()
        {
            // Arrange
            this.SetupRepository(this.transactions);
            this.SetupMapper(this.transactionsDTOs);

            var handler = new GetAllTransactLinksHandler(this.mockRepo.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizerCannotFind.Object);

            // Act
            var result = await handler.Handle(new GetAllTransactLinksQuery(), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.IsType<List<TransactLinkDto>>(result.Value),
                () => Assert.True(result.Value.Count() == this.transactions.Count));
        }

        [Fact]
        public async Task Error()
        {
            // Arrange
            this.SetupRepository(null);
            this.SetupMapper(new List<TransactLinkDto>());

            var expectedError = $"Cannot find any transaction link";
            this.mockLocalizerCannotFind.Setup(localizer => localizer["CannotFindAnyTransactionLink"])
                .Returns(new LocalizedString("CannotFindAnyTransactionLink", expectedError));

            var handler = new GetAllTransactLinksHandler(this.mockRepo.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizerCannotFind.Object);

            // Act
            var result = await handler.Handle(new GetAllTransactLinksQuery(), CancellationToken.None);

            // Assert
            Assert.Equal(expectedError, result.Errors.Single().Message);
        }

        private void SetupRepository(List<TransactionLink> returnList)
        {
            this.mockRepo.Setup(repo => repo.TransactLinksRepository.GetAllAsync(
                It.IsAny<Expression<Func<TransactionLink, bool>>>(), It.IsAny<Func<IQueryable<TransactionLink>,
                IIncludableQueryable<TransactionLink, object>>>())).ReturnsAsync(returnList);
        }

        private void SetupMapper(List<TransactLinkDto> returnList)
        {
            this.mockMapper.Setup(x => x.Map<IEnumerable<TransactLinkDto>>(It.IsAny<IEnumerable<object>>())).Returns(returnList);
        }
    }
}