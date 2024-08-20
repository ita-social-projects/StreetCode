using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Sources;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Sources.SourceLink.Create;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.SourcesTests
{
    public class CreateCategoryTest
    {
        private readonly Mock<IRepositoryWrapper> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly Mock<IStringLocalizer<FailedToCreateSharedResource>> _mockLocalizerFailedToCreate;
        private readonly Mock<IStringLocalizer<CannotConvertNullSharedResource>> _mockLocalizerConvertNull;

        public CreateCategoryTest()
        {
            this._mockRepository = new ();
            this._mockMapper = new ();
            this._mockLogger = new Mock<ILoggerService>();
            this._mockLocalizerConvertNull = new Mock<IStringLocalizer<CannotConvertNullSharedResource>>();
            this._mockLocalizerFailedToCreate = new Mock<IStringLocalizer<FailedToCreateSharedResource>>();
        }

        [Fact]
        public async Task CreateCategoryHandler_TitleAlreadyExists_ReturnsErrorMessage()
        {
            // Arrange
            string title = "ExistingTitle";
            var existingCategory = GetCategory(1, title);
            var newCategoryDto = GetCategoryDTO();
            newCategoryDto.Title = title;

            this.SetupMapper(existingCategory, newCategoryDto);

            var handler = new CreateCategoryHandler(
                this._mockRepository.Object,
                this._mockMapper.Object,
                this._mockLogger.Object,
                this._mockLocalizerFailedToCreate.Object,
                this._mockLocalizerConvertNull.Object);

            this._mockRepository.Setup(p => p.SourceCategoryRepository
                .GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<DAL.Entities.Sources.SourceLinkCategory, bool>>>(),
                    It.IsAny<Func<IQueryable<DAL.Entities.Sources.SourceLinkCategory>,
                    IIncludableQueryable<DAL.Entities.Sources.SourceLinkCategory, object>>>()))
                .ReturnsAsync(existingCategory);

            var expectedError = $"Title: {newCategoryDto.Title} already exists";

            // Act
            var result = await handler.Handle(new CreateCategoryCommand(newCategoryDto), CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(expectedError, result.Errors[0].Message);
        }

        [Fact]
        public async Task CreateCategoryHandler_InvalidCategoryValidation_ReturnsErrorMessage()
        {
            // Arrange
            var invalidCategoryDto = GetCategoryDTO();
            invalidCategoryDto.Title = string.Empty;

            var handler = new CreateCategoryHandler(
                this._mockRepository.Object,
                this._mockMapper.Object,
                this._mockLogger.Object,
                this._mockLocalizerFailedToCreate.Object,
                this._mockLocalizerConvertNull.Object);

            this._mockRepository.Setup(p => p.SourceCategoryRepository
                .GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<DAL.Entities.Sources.SourceLinkCategory, bool>>>(),
                    It.IsAny<Func<IQueryable<DAL.Entities.Sources.SourceLinkCategory>,
                    IIncludableQueryable<DAL.Entities.Sources.SourceLinkCategory, object>>>()))
                .ReturnsAsync(null as DAL.Entities.Sources.SourceLinkCategory);

            var expectedError = "Title cannot be empty.";

            // Act
            var result = await handler.Handle(new CreateCategoryCommand(invalidCategoryDto), CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(expectedError, result.Errors[0].Message);
        }

        [Fact]
        public async Task CreateCategoryHandler_NonExistingImageId_ReturnsErrorMessage()
        {
            // Arrange
            var categoryDto = GetCategoryDTO();
            var existingCategory = GetCategory(1, "Title");
            categoryDto.Title = "Title";

            categoryDto.ImageId = 999;

            this.SetupMapper(existingCategory, categoryDto);

            var handler = new CreateCategoryHandler(
                this._mockRepository.Object,
                this._mockMapper.Object,
                this._mockLogger.Object,
                this._mockLocalizerFailedToCreate.Object,
                this._mockLocalizerConvertNull.Object);

            this._mockRepository.Setup(x => x.ImageRepository.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<Image, bool>>>(),
                    It.IsAny<Func<IQueryable<Image>, IIncludableQueryable<Image, object>>>()))
                .ReturnsAsync((Image?)null);

            this._mockRepository.Setup(p => p.SourceCategoryRepository
                .GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<DAL.Entities.Sources.SourceLinkCategory, bool>>>(),
                    It.IsAny<Func<IQueryable<DAL.Entities.Sources.SourceLinkCategory>,
                    IIncludableQueryable<DAL.Entities.Sources.SourceLinkCategory, object>>>()))
                .ReturnsAsync(null as DAL.Entities.Sources.SourceLinkCategory);

            var expectedError = $"Cannot find an image with corresponding id: {categoryDto.ImageId}";

            // Act
            var result = await handler.Handle(new CreateCategoryCommand(categoryDto), CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(expectedError, result.Errors[0].Message);
        }

        [Fact]
        public async Task CreateCategoryHandler_SaveChangesFailed_ReturnsErrorMessage()
        {
            // Arrange
            this.SetupCreateRepository(-1);
            var categoryDto = GetCategoryDTO();
            this.SetupMapper(GetCategory(1, "Title"), categoryDto);

            var handler = new CreateCategoryHandler(
                this._mockRepository.Object,
                this._mockMapper.Object,
                this._mockLogger.Object,
                this._mockLocalizerFailedToCreate.Object,
                this._mockLocalizerConvertNull.Object);

            this._mockRepository.Setup(x => x.ImageRepository.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<Image, bool>>>(),
                    It.IsAny<Func<IQueryable<Image>, IIncludableQueryable<Image, object>>>()))
                .ReturnsAsync(new Image());

            this._mockRepository.Setup(p => p.SourceCategoryRepository
                .GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<DAL.Entities.Sources.SourceLinkCategory, bool>>>(),
                    It.IsAny<Func<IQueryable<DAL.Entities.Sources.SourceLinkCategory>,
                    IIncludableQueryable<DAL.Entities.Sources.SourceLinkCategory, object>>>()))
                .ReturnsAsync(null as DAL.Entities.Sources.SourceLinkCategory);

            var expectedError = "Failed to create category";

            // Act
            var result = await handler.Handle(new CreateCategoryCommand(categoryDto), CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(expectedError, result.Errors[0].Message);
        }

        [Fact]
        public async Task CreateCategoryHandler_SuccessfulCreation_ReturnsSuccess()
        {
            // Arrange
            string title = "NewTitle";
            var newCategory = GetCategory(1, title);
            var newCategoryDto = GetCategoryDTO();
            newCategoryDto.Title = title;

            this.SetupCreateRepository(1); // Simulate successful SaveChangesAsync
            this.SetupMapper(newCategory, newCategoryDto);

            var handler = new CreateCategoryHandler(
                this._mockRepository.Object,
                this._mockMapper.Object,
                this._mockLogger.Object,
                this._mockLocalizerFailedToCreate.Object,
                this._mockLocalizerConvertNull.Object);

            this._mockRepository.Setup(x => x.ImageRepository.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<Image, bool>>>(),
                    It.IsAny<Func<IQueryable<Image>, IIncludableQueryable<Image, object>>>()))
                .ReturnsAsync(new Image());

            this._mockRepository.Setup(p => p.SourceCategoryRepository
                .GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<DAL.Entities.Sources.SourceLinkCategory, bool>>>(),
                    It.IsAny<Func<IQueryable<DAL.Entities.Sources.SourceLinkCategory>,
                    IIncludableQueryable<DAL.Entities.Sources.SourceLinkCategory, object>>>()))
          .ReturnsAsync(null as DAL.Entities.Sources.SourceLinkCategory);

            // Act
            var result = await handler.Handle(new CreateCategoryCommand(newCategoryDto), CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
        }

        private static DAL.Entities.Sources.SourceLinkCategory GetCategory(int imageId = 0, string? title = null)
        {
            return new DAL.Entities.Sources.SourceLinkCategory()
            {
                Id = 1,
                ImageId = imageId,
                Title = title,
            };
        }

        private static SourceLinkCategoryCreateDTO GetCategoryDTO()
        {
            return new SourceLinkCategoryCreateDTO()
            {
                ImageId = 1,
                Title = "Title",
            };
        }

        private void SetupCreateRepository(int returnNumber)
        {
            this._mockRepository.Setup(x => x.SourceCategoryRepository.Create(It.IsAny<DAL.Entities.Sources.SourceLinkCategory>()));
            this._mockRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(returnNumber);
        }

        private void SetupMapper(DAL.Entities.Sources.SourceLinkCategory testCategory, SourceLinkCategoryCreateDTO testCategoryDTO)
        {
            this._mockMapper.Setup(x => x.Map<DAL.Entities.Sources.SourceLinkCategory>(It.IsAny<SourceLinkCategoryCreateDTO>()))
                .Returns(testCategory);
            this._mockMapper.Setup(x => x.Map<SourceLinkCategoryCreateDTO>(It.IsAny<DAL.Entities.Sources.SourceLinkCategory>()))
                .Returns(testCategoryDTO);
        }
    }
}
