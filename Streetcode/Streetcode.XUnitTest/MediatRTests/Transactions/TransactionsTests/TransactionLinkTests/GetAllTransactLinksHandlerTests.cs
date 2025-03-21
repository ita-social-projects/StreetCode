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
        private readonly Mock<IRepositoryWrapper> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> _mockLocalizerCannotFind;

        private readonly List<TransactionLink> _transactions = new()
        {
            new ()
            {
                Id = 1,
                Url = "URL",
                UrlTitle = "Title",
                StreetcodeId = 1,
            },
            new ()
            {
                Id = 2,
                Url = "URL2",
                UrlTitle = "Title2",
                StreetcodeId = 2,
            },
        };

        private readonly List<TransactLinkDTO> _transactionsDtOs = new()
        {
            new ()
            {
                Id = 1,
                Url = "URL",
                QrCodeUrl = "URL",
                StreetcodeId = 1,
            },
            new ()
            {
                Id = 2,
                Url = "URL2",
                QrCodeUrl = "URL2",
                StreetcodeId = 2,
            },
        };

        public GetAllTransactLinksHandlerTests()
        {
            _mockMapper = new Mock<IMapper>();
            _mockRepo = new Mock<IRepositoryWrapper>();
            _mockLogger = new Mock<ILoggerService>();
            _mockLocalizerCannotFind = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        }

        [Fact]
        public async Task NotEmpty_List()
        {
            // Arrange
            SetupRepository(_transactions);
            SetupMapper(_transactionsDtOs);

            var handler = new GetAllTransactLinksHandler(_mockRepo.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizerCannotFind.Object);

            // Act
            var result = await handler.Handle(new GetAllTransactLinksQuery(), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.IsType<List<TransactLinkDTO>>(result.Value),
                () => Assert.True(result.Value.Count() == _transactions.Count));
        }

        [Fact]
        public async Task Error()
        {
            // Arrange
            SetupRepository(null);
            SetupMapper(new List<TransactLinkDTO>());

            var expectedError = $"Cannot find any transaction link";
            _mockLocalizerCannotFind.Setup(localizer => localizer["CannotFindAnyTransactionLink"])
                .Returns(new LocalizedString("CannotFindAnyTransactionLink", expectedError));

            var handler = new GetAllTransactLinksHandler(_mockRepo.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizerCannotFind.Object);

            // Act
            var result = await handler.Handle(new GetAllTransactLinksQuery(), CancellationToken.None);

            // Assert
            Assert.Equal(expectedError, result.Errors.Single().Message);
        }

        private void SetupRepository(List<TransactionLink>? returnList)
        {
            _mockRepo
                .Setup(repo => repo.TransactLinksRepository.GetAllAsync(
                    It.IsAny<Expression<Func<TransactionLink, bool>>>(), It.IsAny<Func<IQueryable<TransactionLink>,
                    IIncludableQueryable<TransactionLink, object>>>()))
                .ReturnsAsync(returnList!);
        }

        private void SetupMapper(List<TransactLinkDTO> returnList)
        {
            _mockMapper
                .Setup(x => x.Map<IEnumerable<TransactLinkDTO>>(
                    It.IsAny<IEnumerable<object>>()))
                .Returns(returnList);
        }
    }
}