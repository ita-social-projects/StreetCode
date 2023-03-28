using AutoMapper;
using Moq;
using Streetcode.BLL.DTO.Transactions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.BLL.DTO.Transactions;
using Streetcode.BLL.MediatR.AdditionalContent.Tag.GetAll;
using Streetcode.BLL.MediatR.Streetcode.Fact.GetById;
using Streetcode.BLL.MediatR.Transactions.TransactionLink.GetAll;
using Streetcode.BLL.MediatR.Transactions.TransactionLink.GetByStreetcodeId;
using Streetcode.DAL.Entities.AdditionalContent;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Entities.Transactions;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Streetcode.BLL.MediatR.Transactions.TransactionLink.GetById;

namespace Streetcode.XUnitTest.MediatRTests.Transactions.TransactionsTests.TransactionLinkTests
{
    public class GetTransactLinkByIdHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        public GetTransactLinkByIdHandlerTests()
        {

            _mockMapper = new Mock<IMapper>();
            _mockRepo = new Mock<IRepositoryWrapper>();

        }

        async Task SetupRepository(int id)
        {

            _mockRepo.Setup(x => x.TransactLinksRepository.GetFirstOrDefaultAsync(
               It.IsAny<Expression<Func<TransactionLink, bool>>>(), It.IsAny<Func<IQueryable<TransactionLink>,
                IIncludableQueryable<TransactionLink, object>>>())).ReturnsAsync(getTransaction(id));

        }

        async Task SetupMapper(int id)
        {

            _mockMapper.Setup(x => x.Map<TransactLinkDTO>(It.IsAny<TransactionLink>())).Returns(getTransactionDTO(id));

        }

        [Theory]
        [InlineData(1)]
        public async Task ExistingId(int id)
        {
            
            //Arrange
            await SetupMapper(id);
            await SetupRepository(id);

            var handler = new GetTransactLinkByIdHandler(_mockRepo.Object, _mockMapper.Object);

            //Act
            var result = await handler.Handle(new GetTransactLinkByIdQuery(id), CancellationToken.None);

            //Assert
            Assert.Multiple(
                () => Assert.NotNull(result),
                () => Assert.True(result.IsSuccess),
                () => Assert.Equal(result.Value.Id, id)
            );

        }

        [Theory]
        [InlineData(1)]
        public async Task NotExistingId(int id)
        {

            //Arrange
            _mockRepo.Setup(x => x.TransactLinksRepository.GetFirstOrDefaultAsync(
              It.IsAny<Expression<Func<TransactionLink, bool>>>(), It.IsAny<Func<IQueryable<TransactionLink>,
               IIncludableQueryable<TransactionLink, object>>>())).ReturnsAsync(returnNull(id));

            _mockMapper.Setup(x => x.Map<TransactLinkDTO>(It.IsAny<TransactionLink>())).Returns(returnNullDTO(id));

            var expectedError = $"Cannot find any transaction link with corresponding id: {id}";

            var handler = new GetTransactLinkByIdHandler(_mockRepo.Object, _mockMapper.Object);

            //Act
            var result = await handler.Handle(new GetTransactLinkByIdQuery(id), CancellationToken.None);

            //Assert
            Assert.Multiple(
                () => Assert.NotNull(result),
                () => Assert.True(result.IsFailed),
                () => Assert.Equal(expectedError, result.Errors.First().Message)
            );

        }

        [Theory]
        [InlineData(1)]
        public async Task CorrectType(int id)
        {

            //Arrange
            await SetupMapper(id);
            await SetupRepository(id);

            var handler = new GetTransactLinkByIdHandler(_mockRepo.Object, _mockMapper.Object);

            //Act
            var result = await handler.Handle(new GetTransactLinkByIdQuery(id), CancellationToken.None);

            //Assert
            Assert.Multiple(
                () => Assert.NotNull(result.ValueOrDefault),
                () => Assert.IsType<TransactLinkDTO>(result.ValueOrDefault)
            );

        }

        private static TransactionLink returnNull(int id)
        {

            return null;

        }

        private static TransactLinkDTO returnNullDTO(int id)
        {

            return null;

        }

        private static TransactionLink getTransaction(int id)
        {

            return new TransactionLink
            {
                Id = id
            };

        }

        private static TransactLinkDTO getTransactionDTO(int id)
        {

            return new TransactLinkDTO
            {
                Id = id
            };

        }
    }
}
