using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Transactions;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Transactions.TransactionLink.GetByStreetcodeId;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Entities.Transactions;
using Streetcode.DAL.Repositories.Interfaces.Base;

using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Transactions.TransactionsTests.TransactionLinkTests
{
    public class GetTransactLinkByStreetcodeIdHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> mockRepo;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<ILoggerService> mockLogger;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> mockLocalizer;
        private readonly StreetcodeContent? nullStreetcodeContent = null;
        private readonly TransactionLink? nullValue = null;
        private readonly TransactLinkDTO? nullValueDTO = null;

        public GetTransactLinkByStreetcodeIdHandlerTests()
        {
            this.mockMapper = new Mock<IMapper>();
            this.mockRepo = new Mock<IRepositoryWrapper>();
            this.mockLogger = new Mock<ILoggerService>();
            this.mockLocalizer = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        }

        [Theory]
        [InlineData(1)]
        public async Task ExistingId(int id)
        {
            // Arrange
            this.SetupMapper(id);
            this.SetupRepository(id);

            var handler = new GetTransactLinkByStreetcodeIdHandler(this.mockRepo.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizer.Object);

            // Act
            var result = await handler.Handle(new GetTransactLinkByStreetcodeIdQuery(id), CancellationToken.None);

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
            this.mockRepo.Setup(x => x.TransactLinksRepository
                .GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<TransactionLink, bool>>>(),
                    It.IsAny<Func<IQueryable<TransactionLink>,
                    IIncludableQueryable<TransactionLink, object>>>()))
                .ReturnsAsync(this.nullValue);

            this.mockRepo.Setup(x => x.StreetcodeRepository
                .GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<StreetcodeContent, bool>>>(),
                    It.IsAny<Func<IQueryable<StreetcodeContent>,
                    IIncludableQueryable<StreetcodeContent, object>>>()))
                .ReturnsAsync(this.nullStreetcodeContent);

            this.mockMapper
                .Setup(x => x.Map<TransactLinkDTO?>(It.IsAny<TransactionLink>()))
                .Returns(this.nullValueDTO);

            var expectedError = $"Cannot find a transaction link by a streetcode id: {id}";
            this.mockLocalizer.Setup(x => x["CannotFindTransactionLinkByStreetcodeIdBecause", id])
               .Returns(new LocalizedString("CannotFindTransactionLinkByStreetcodeIdBecause", expectedError));

            var handler = new GetTransactLinkByStreetcodeIdHandler(this.mockRepo.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizer.Object);

            // Act
            var result = await handler.Handle(new GetTransactLinkByStreetcodeIdQuery(id), CancellationToken.None);

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
            this.SetupMapper(id);
            this.SetupRepository(id);

            var handler = new GetTransactLinkByStreetcodeIdHandler(this.mockRepo.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizer.Object);

            // Act
            var result = await handler.Handle(new GetTransactLinkByStreetcodeIdQuery(id), CancellationToken.None);

            // Assert
            Assert.Multiple(
                   () => Assert.NotNull(result.ValueOrDefault),
                   () => Assert.IsType<TransactLinkDTO>(result.ValueOrDefault));
        }

        private void SetupRepository(int id)
        {
            this.mockRepo.Setup(x => x.TransactLinksRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<TransactionLink, bool>>>(),
                It.IsAny<Func<IQueryable<TransactionLink>, IIncludableQueryable<TransactionLink, object>>>()))
            .ReturnsAsync(new TransactionLink() { Id = id });
        }

        private void SetupMapper(int id)
        {
            this.mockMapper.Setup(x => x.Map<TransactLinkDTO>(It.IsAny<TransactionLink>())).Returns(new TransactLinkDTO() { Id = id });
        }
    }
}