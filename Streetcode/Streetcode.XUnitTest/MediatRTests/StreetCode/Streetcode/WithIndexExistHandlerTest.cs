using System.Linq.Expressions;
using AutoMapper;
using FluentResults;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.WithIndexExist;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Repositories.Interfaces.Base;
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
            this._repository = new Mock<IRepositoryWrapper>();
            this._mapper = new Mock<IMapper>();
            this._mockLogger = new Mock<ILoggerService>();
        }

        [Theory]
        [InlineData(1)]
        public async Task ShouldReturnSuccesfully(int id)
        {
            // Arrange
            this._repository.Setup(x => x.StreetcodeRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<StreetcodeContent, bool>>>(),
                It.IsAny<Func<IQueryable<StreetcodeContent>,
                IIncludableQueryable<StreetcodeContent, object>>>()))
            .ReturnsAsync(this.GetStreetCodeContent(id));

            var handler = new StreetcodeWithIndexExistHandler(this._repository.Object, this._mapper.Object, this._mockLogger.Object);

            // Act
            var result = await handler.Handle(new StreetcodeWithIndexExistQuery(id), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.NotNull(result),
                () => Assert.IsAssignableFrom<Result<bool>>(result),
                () => Assert.True(result.Value));
        }

        [Theory]
        [InlineData(1)]
        public async Task ShouldReturnFalse_NotExistingId(int id)
        {
            // Arrange
            this._repository.Setup(x => x.StreetcodeRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<StreetcodeContent, bool>>>(),
                It.IsAny<Func<IQueryable<StreetcodeContent>,
                IIncludableQueryable<StreetcodeContent, object>>>()))
            .ReturnsAsync(this.GetNull());

            var handler = new StreetcodeWithIndexExistHandler(this._repository.Object, this._mapper.Object, this._mockLogger.Object);

            // Act
            var result = await handler.Handle(new StreetcodeWithIndexExistQuery(id), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.NotNull(result),
                () => Assert.IsAssignableFrom<Result<bool>>(result),
                () => Assert.False(result.Value));
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
