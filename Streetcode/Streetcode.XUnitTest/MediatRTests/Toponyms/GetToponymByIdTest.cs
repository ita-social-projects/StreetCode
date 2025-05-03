using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Toponyms;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Toponyms.GetById;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Toponyms;
using Streetcode.DAL.Enums;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Toponyms
{
    public class GetToponymByIdTest
    {
        private readonly Mock<IRepositoryWrapper> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> _mockLocalizerCannotFind;

        public GetToponymByIdTest()
        {
            _mockRepository = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILoggerService>();
            _mockLocalizerCannotFind = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        }

        [Fact]
        public async Task GetById_ReturnsSuccess()
        {
            // Arrange
            var testToponym = GetToponym();
            var testToponymDto = GetToponymDto();

            SetupRepository(testToponym);
            SetupMapper(testToponymDto);

            var handler = new GetToponymByIdHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizerCannotFind.Object);

            // Act
            var result = await handler.Handle(new GetToponymByIdQuery(testToponym.Id, UserRole.User), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.NotNull(result),
                () => Assert.True(result.IsSuccess),
                () => Assert.IsType<ToponymDTO>(result.Value),
                () => Assert.Equal(result.Value.Id, testToponym.Id));
        }

        [Fact]
        public async Task GetByIdIncorrect_ReturnsError()
        {
            // Arrange
            var incorrectId = -1;
            var expectedError = $"Cannot find any toponym with corresponding id: {incorrectId}";

            SetupRepository(null);
            SetupLocalizer(incorrectId);

            var handler = new GetToponymByIdHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizerCannotFind.Object);

            // Act
            var result = await handler.Handle(new GetToponymByIdQuery(incorrectId, UserRole.User), CancellationToken.None);

            // Assert
            Assert.Multiple(
            () => Assert.True(result.IsFailed),
            () => Assert.Equal(expectedError, result.Errors[0].Message));
        }

        private static Toponym GetToponym()
        {
            return new Toponym
            {
                Id = 1,
            };
        }

        private static ToponymDTO GetToponymDto()
        {
            return new ToponymDTO
            {
                Id = 1,
            };
        }

        private void SetupRepository(Toponym? returnValue)
        {
            _mockRepository.Setup(x => x.ToponymRepository
            .GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Toponym, bool>>>(),
                It.IsAny<Func<IQueryable<Toponym>,
                IIncludableQueryable<Toponym, object>>>()))
            .ReturnsAsync(returnValue);
        }

        private void SetupMapper(ToponymDTO returnValue)
        {
            _mockMapper
                .Setup(x => x.Map<ToponymDTO>(It.IsAny<Toponym>()))
                .Returns(returnValue);
        }

        private void SetupLocalizer(int incorrectId)
        {
            _mockLocalizerCannotFind.Setup(x => x[It.IsAny<string>(), It.IsAny<object>()]).Returns((string key, object[] args) =>
            {
                if (args != null && args.Length > 0 && args[0] is int)
                {
                    return new LocalizedString(key, $"Cannot find any toponym with corresponding id: {incorrectId}");
                }

                return new LocalizedString(key, "Cannot find any toponym with unknown id");
            });
        }
    }
}