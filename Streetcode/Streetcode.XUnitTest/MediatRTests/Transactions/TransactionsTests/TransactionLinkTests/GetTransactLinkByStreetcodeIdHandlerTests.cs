using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using MockQueryable.Moq;
using Moq;
using Streetcode.BLL.DTO.Transactions;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Transactions.TransactionLink.GetByStreetcodeId;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Entities.Transactions;
using Streetcode.DAL.Enums;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Transactions.TransactionsTests.TransactionLinkTests
{
    public class GetTransactLinkByStreetcodeIdHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly MockCannotFindLocalizer _mockLocalizer;

        public GetTransactLinkByStreetcodeIdHandlerTests()
        {
            _mockMapper = new Mock<IMapper>();
            _mockRepo = new Mock<IRepositoryWrapper>();
            _mockLogger = new Mock<ILoggerService>();
            _mockLocalizer = new MockCannotFindLocalizer();
        }

        [Theory]
        [InlineData(1)]
        public async Task ExistingId(int id)
        {
            // Arrange
            SetupMapper(new TransactLinkDTO() { Id = id });
            SetupRepository(new TransactionLink() { Id = id }, GetStreetcodeList());

            var handler = new GetTransactLinkByStreetcodeIdHandler(_mockRepo.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizer);

            // Act
            var result = await handler.Handle(new GetTransactLinkByStreetcodeIdQuery(id, UserRole.User), CancellationToken.None);

            // Assert
            Assert.Multiple(
                   () => Assert.NotNull(result),
                   () => Assert.True(result.IsSuccess),
                   () => Assert.Equal(result.Value?.Id, id));
        }

        [Theory]
        [InlineData(1)]
        public async Task NotExistingId(int id)
        {
            // Arrange
            SetupRepository(null, new List<StreetcodeContent>());
            SetupMapper(null);

            var expectedError = _mockLocalizer["CannotFindTransactionLinkByStreetcodeIdBecause", id].Value;

            var handler = new GetTransactLinkByStreetcodeIdHandler(_mockRepo.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizer);

            // Act
            var result = await handler.Handle(new GetTransactLinkByStreetcodeIdQuery(id, UserRole.User), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.NotNull(result),
                () => Assert.True(result.IsFailed),
                () => Assert.Equal(expectedError, result.Errors[0].Message));
        }

        [Theory]
        [InlineData(1)]
        public async Task CorrectType(int id)
        {
            // Arrange
            SetupMapper(new TransactLinkDTO() { Id = id });
            SetupRepository(new TransactionLink() { Id = id }, GetStreetcodeList());

            var handler = new GetTransactLinkByStreetcodeIdHandler(_mockRepo.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizer);

            // Act
            var result = await handler.Handle(new GetTransactLinkByStreetcodeIdQuery(id, UserRole.User), CancellationToken.None);

            // Assert
            Assert.Multiple(
                   () => Assert.NotNull(result.ValueOrDefault),
                   () => Assert.IsType<TransactLinkDTO>(result.ValueOrDefault));
        }

        private void SetupRepository(TransactionLink transactionLink, List<StreetcodeContent> streetcodeListUserCanAccess)
        {
            _mockRepo.Setup(x => x.TransactLinksRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<TransactionLink, bool>>>(),
                It.IsAny<Func<IQueryable<TransactionLink>, IIncludableQueryable<TransactionLink, object>>>()))
            .ReturnsAsync(transactionLink);

            _mockRepo.Setup(repo => repo.StreetcodeRepository
                    .FindAll(
                        It.IsAny<Expression<Func<StreetcodeContent, bool>>>(),
                        It.IsAny<Func<IQueryable<StreetcodeContent>,
                            IIncludableQueryable<StreetcodeContent, object>>>()))
                .Returns(streetcodeListUserCanAccess.AsQueryable().BuildMockDbSet().Object);
        }

        private void SetupMapper(TransactLinkDTO transactLinkDto)
        {
            _mockMapper.Setup(x => x.Map<TransactLinkDTO>(It.IsAny<TransactionLink>())).Returns(transactLinkDto);
        }

        private static List<StreetcodeContent> GetStreetcodeList()
        {
            return new List<StreetcodeContent>
            {
                new StreetcodeContent
                {
                    Id = 1,
                },
            };
        }
    }
}