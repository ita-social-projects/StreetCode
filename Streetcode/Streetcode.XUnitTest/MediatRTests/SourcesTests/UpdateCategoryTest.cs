using AutoMapper;
using Moq;
using Streetcode.BLL.DTO.Sources;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.MediatR.Sources.SourceLink.Update;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System.Linq.Expressions;
using Xunit;
using Streetcode.DAL.Entities.Media.Images;
using Microsoft.EntityFrameworkCore.Query;
using System.Threading.Tasks;
using System.Threading;
using Streetcode.BLL.Interfaces.Logging;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.SharedResource;
using MediatR;
using Streetcode.BLL.MediatR.Sources.SourceLink.Create;

namespace Streetcode.XUnitTest.MediatRTests.SourcesTests
{
    public class UpdateCategoryTests
    {
        private Mock<IRepositoryWrapper> _mockRepository;
        private Mock<IMapper> _mockMapper;
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly Mock<IStringLocalizer<FailedToUpdateSharedResource>> _mockLocalizerFailedToUpdate;
        private readonly Mock<IStringLocalizer<CannotConvertNullSharedResource>> _mockLocalizerConvertNull;

        public UpdateCategoryTests()
        {
            _mockRepository = new();
            _mockMapper = new();
            _mockLogger = new Mock<ILoggerService>();
            _mockLocalizerConvertNull = new Mock<IStringLocalizer<CannotConvertNullSharedResource>>();
            _mockLocalizerFailedToUpdate = new Mock<IStringLocalizer<FailedToUpdateSharedResource>>();
        }

        [Theory]
        [InlineData(1)]
        public async Task ShouldReturnSuccessfully_WhenUpdated(int returnNumber)
        {
            // Arrange
            var testCategory = GetCategory(1);
            var testCategoryDTO = GetCategoryDTO();

            SetupUpdateRepository(returnNumber);
            SetupMapper(testCategory, testCategoryDTO);
            SetupImageRepository();

            var handler = new UpdateCategoryHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizerFailedToUpdate.Object, _mockLocalizerConvertNull.Object);

            // Act
            var result = await handler.Handle(new UpdateCategoryCommand(testCategoryDTO), CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Empty(result.Errors); 
            Assert.Equal(Unit.Value, result.Value); 
        }


        [Theory]
        [InlineData(1)]
        public async Task ShouldThrowException_TryMapNullRequest(int returnNumber)
        {
            // Arrange
            var expectedError = "Cannot convert null to category";
            _mockLocalizerConvertNull.Setup(x => x["CannotConvertNullToCategory"])
            .Returns(new LocalizedString("CannotConvertNullToCategory", expectedError));
            SetupUpdateRepository(returnNumber);
            SetupMapperWithNullCategory();

            var handler = new UpdateCategoryHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizerFailedToUpdate.Object, _mockLocalizerConvertNull.Object);

            // Act
            var result = await handler.Handle(new UpdateCategoryCommand(GetCategoryDTOWithNotExistId()), CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(expectedError, result.Errors.First().Message);
        }

        [Theory]
        [InlineData(-1)]
        public async Task ShouldThrowException_SaveChangesAsyncIsNotSuccessful(int returnNumber)
        {
            // Arrange
            var testCategory = GetCategory(1);
            var testCategoryDTO = GetCategoryDTO();

            SetupUpdateRepository(returnNumber);
            SetupMapper(testCategory, testCategoryDTO);
            SetupImageRepository();

            var expectedError = "Failed to update category";
            _mockLocalizerFailedToUpdate.Setup(x => x["FailedToUpdateCategory"])
            .Returns(new LocalizedString("FailedToUpdateCategory", expectedError));
            var handler = new UpdateCategoryHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizerFailedToUpdate.Object, _mockLocalizerConvertNull.Object);

            // Act
            var result = await handler.Handle(new UpdateCategoryCommand(testCategoryDTO), CancellationToken.None);

            // Assert
            Assert.True(result.IsFailed);
            Assert.Equal(expectedError, result.Errors.First().Message);
        }

        [Theory]
        [InlineData(1)]
        public async Task ShouldReturnSuccessfully_TypeIsCorrect(int returnNumber)
        {
            // Arrange
            SetupCreateRepository(returnNumber);
            SetupMapper(GetCategory(1), GetCategoryDTO());
            SetupImageRepository();

            var handler = new UpdateCategoryHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizerFailedToUpdate.Object, _mockLocalizerConvertNull.Object);

            // Act
            var result = await handler.Handle(new UpdateCategoryCommand(GetCategoryDTO()), CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
        }
        [Fact]
        public async Task ShouldReturnFail_ImageIdIsZero()
        {
            // Arrange
            string expectedErrorMessage = "Invalid ImageId Value";
            var teamMember = GetCategory();
            SetupMapper(GetCategory(), GetCategoryDTO());
            var handler = new UpdateCategoryHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizerFailedToUpdate.Object, _mockLocalizerConvertNull.Object);

            // Act
            var result = await handler.Handle(new UpdateCategoryCommand(new SourceLinkCategoryDTO()), CancellationToken.None);
            // Assert
            Assert.Multiple(
                () => Assert.True(result.IsFailed),
                () => Assert.Equal(expectedErrorMessage, result.Errors.First().Message)
            );

        }

        private void SetupUpdateRepository(int returnNumber)
        {
            _mockRepository.Setup(x => x.SourceCategoryRepository.Update(It.IsAny<DAL.Entities.Sources.SourceLinkCategory>()));
            _mockRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(returnNumber);
        }

        private void SetupCreateRepository(int returnNumber)
        {
            _mockRepository.Setup(x => x.SourceCategoryRepository.Create(GetCategory(1)));
            _mockRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(returnNumber);
        }

        private void SetupMapper(DAL.Entities.Sources.SourceLinkCategory testCategory, SourceLinkCategoryDTO testCategoryDTO)
        {
            _mockMapper.Setup(x => x.Map<DAL.Entities.Sources.SourceLinkCategory>(It.IsAny<SourceLinkCategoryDTO>()))
                .Returns(testCategory);
            _mockMapper.Setup(x => x.Map<SourceLinkCategoryDTO>(It.IsAny<DAL.Entities.Sources.SourceLinkCategory>()))
                .Returns(testCategoryDTO);
        }

        private void SetupMapperWithNullCategory()
        {
            _mockMapper.Setup(x => x.Map<DAL.Entities.Sources.SourceLinkCategory>(It.IsAny<SourceLinkCategoryDTO>()))
                .Returns(GetCategoryWithNotExistId());
        }

        private void SetupImageRepository()
        {
            _mockRepository.Setup(x => x.ImageRepository.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<Image, bool>>>(),
                    It.IsAny<Func<IQueryable<Image>, IIncludableQueryable<Image, object>>>()
                )).ReturnsAsync((Image)null);
        }

        private static DAL.Entities.Sources.SourceLinkCategory GetCategory(int imageId = 0)
        {
            return new DAL.Entities.Sources.SourceLinkCategory()
            {
                Id = 1,
                ImageId = imageId
            };
        }

        private static SourceLinkCategoryDTO GetCategoryDTO()
        {
            return new SourceLinkCategoryDTO();
        }

        private static DAL.Entities.Sources.SourceLinkCategory GetCategoryWithNotExistId() => null;

        private static SourceLinkCategoryDTO GetCategoryDTOWithNotExistId() => null;
    }
}
