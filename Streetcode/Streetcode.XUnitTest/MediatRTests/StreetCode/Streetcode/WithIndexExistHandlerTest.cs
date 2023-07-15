using AutoMapper;
using FluentResults;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.WithIndexExist;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System.Linq.Expressions;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Streetcode
{
    public class WithIndexExistHandlerTest
    {
        private readonly Mock<IRepositoryWrapper> _repository;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<ILoggerService> _mockLogger;
        public WithIndexExistHandlerTest()
        {
            _repository = new Mock<IRepositoryWrapper>();
            _mapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILoggerService>();
        }
        [Theory]
        [InlineData(1)]
        public async Task ShouldReturnSuccesfully(int id)
        {
            // arrange
            _repository.Setup(x => x.StreetcodeRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<StreetcodeContent, bool>>>(),
                It.IsAny<Func<IQueryable<StreetcodeContent>,
                IIncludableQueryable<StreetcodeContent, object>>>()))
            .ReturnsAsync(GetStreetCodeContent(id));

            var handler = new StreetcodeWithIndexExistHandler(_repository.Object, _mapper.Object, _mockLogger.Object);

            // act

            var result = await handler.Handle(new StreetcodeWithIndexExistQuery(id), CancellationToken.None);

            // assert

            Assert.Multiple(
                () => Assert.NotNull(result),
                () => Assert.IsAssignableFrom<Result<bool>>(result),
                () => Assert.True(result.Value)
            );
        }
        [Theory]
        [InlineData(1)]
        public async Task ShouldReturnFalse_NotExistingId(int id)
        {
            // arrange
            _repository.Setup(x => x.StreetcodeRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<StreetcodeContent, bool>>>(),
                It.IsAny<Func<IQueryable<StreetcodeContent>,
                IIncludableQueryable<StreetcodeContent, object>>>()))
            .ReturnsAsync(GetNull());

            var handler = new StreetcodeWithIndexExistHandler(_repository.Object, _mapper.Object, _mockLogger.Object);

            // act

            var result = await handler.Handle(new StreetcodeWithIndexExistQuery(id), CancellationToken.None);

            // assert

            Assert.Multiple(
                () => Assert.NotNull(result),
                () => Assert.IsAssignableFrom<Result<bool>>(result),
                () => Assert.False(result.Value)
            );
        }
        private StreetcodeContent GetStreetCodeContent(int id)
        {
            return new StreetcodeContent() { Id = id };
        }

        private StreetcodeContent? GetNull()
        {
            return null;
        }
    }
}
