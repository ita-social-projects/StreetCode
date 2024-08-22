using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Sources;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Sources.SourceLink.Update;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.SourcesTests
{
    public class UpdateCategoryTest
    {
        private readonly Mock<IRepositoryWrapper> _mockRepository;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<ILoggerService> mockLogger;
        private readonly Mock<IStringLocalizer<FailedToUpdateSharedResource>> mockLocalizerFailedToUpdate;
        private readonly Mock<IStringLocalizer<CannotConvertNullSharedResource>> mockLocalizerConvertNull;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> mockLocalizerCannotFindSharedResource;

        public UpdateCategoryTest()
        {
            this._mockRepository = new ();
            this.mockMapper = new ();
            this.mockLogger = new Mock<ILoggerService>();
            this.mockLocalizerConvertNull = new Mock<IStringLocalizer<CannotConvertNullSharedResource>>();
            this.mockLocalizerFailedToUpdate = new Mock<IStringLocalizer<FailedToUpdateSharedResource>>();
            this.mockLocalizerCannotFindSharedResource = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        }

        [Theory]
        [InlineData(1)]
        public async Task ShouldThrowException_TryMapNullRequest(int returnNumber)
        {
            // Arrange
            this.SetupCreateRepository(returnNumber);
            string title = "Tested";
            var sourceCategory = GetCategory(1, title);
            var sourceCategoryDto = GetCategoryDTO();
            sourceCategoryDto.Title = title;
            sourceCategoryDto.ImageId = sourceCategory.ImageId;
            sourceCategoryDto.Id = sourceCategory.Id;
            this.SetupMapper(sourceCategory, sourceCategoryDto);

            var handler = new UpdateCategoryHandler(
                this._mockRepository.Object,
                this.mockMapper.Object,
                this.mockLogger.Object,
                this.mockLocalizerFailedToUpdate.Object,
                this.mockLocalizerConvertNull.Object,
                this.mockLocalizerCannotFindSharedResource.Object);

            this._mockRepository
                .Setup(p => p.SourceCategoryRepository.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<DAL.Entities.Sources.SourceLinkCategory, bool>>>(),
                    It.IsAny<Func<IQueryable<DAL.Entities.Sources.SourceLinkCategory>,
                    IIncludableQueryable<DAL.Entities.Sources.SourceLinkCategory, object>>>()))
                .ReturnsAsync(sourceCategory);

            this._mockRepository
                .Setup(x => x.ImageRepository.GetFirstOrDefaultAsync(
                   It.IsAny<Expression<Func<Image, bool>>>(),
                   It.IsAny<Func<IQueryable<Image>, IIncludableQueryable<Image, object>>>()))
                .ReturnsAsync(new Image());

            this.mockMapper.Setup(m => m.Map<DAL.Entities.Sources.SourceLinkCategory?>(It.IsAny<SourceLinkCreateUpdateCategoryDTO>()))
                .Returns((DAL.Entities.Sources.SourceLinkCategory?)null);

            var expectedError = "Cannot convert null to category";
            this.mockLocalizerConvertNull.Setup(x => x["CannotConvertNullToCategory"])
            .Returns(new LocalizedString("CannotConvertNullToCategory", expectedError));

            // Act
            var result = await handler.Handle(new UpdateCategoryCommand(sourceCategoryDto), CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(expectedError, result.Errors[0].Message);
        }

        [Fact]
        public async Task UpdateCategoryHandler_ExistingCategoryIsNull_ReturnsErrorMessage()
        {
            string title = "Tested";
            var sourceCategory = GetCategory(1, title);
            var sourceCategoryDto = GetCategoryDTO();
            sourceCategoryDto.Title = title;
            sourceCategoryDto.ImageId = sourceCategory.ImageId;
            this.SetupMapper(sourceCategory, sourceCategoryDto);

            var handler = new UpdateCategoryHandler(
                this._mockRepository.Object,
                this.mockMapper.Object,
                this.mockLogger.Object,
                this.mockLocalizerFailedToUpdate.Object,
                this.mockLocalizerConvertNull.Object,
                this.mockLocalizerCannotFindSharedResource.Object);

            this._mockRepository
                .Setup(p => p.SourceCategoryRepository.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<DAL.Entities.Sources.SourceLinkCategory, bool>>>(),
                    It.IsAny<Func<IQueryable<DAL.Entities.Sources.SourceLinkCategory>,
                    IIncludableQueryable<DAL.Entities.Sources.SourceLinkCategory, object>>>()))
                .ReturnsAsync((DAL.Entities.Sources.SourceLinkCategory?)null);

            var expectedError = "Cannot find any srcCategory by the corresponding id: 0";
            this.mockLocalizerCannotFindSharedResource.Setup(x => x["CannotFindAnySrcCategoryByTheCorrespondingId", sourceCategoryDto.Id])
            .Returns(new LocalizedString("CannotFindAnySrcCategoryByTheCorrespondingId", expectedError));

            // Act
            var result = await handler.Handle(new UpdateCategoryCommand(sourceCategoryDto), CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(expectedError, result.Errors[0].Message);
        }

        [Theory]
        [InlineData(-1)]
        public async Task ShouldThrowException_SaveChangesAsyncIsNotSuccessful(int returnNumber)
        {
            // Arrange
            this.SetupCreateRepository(returnNumber);
            string title = "Tested";
            var sourceCategory = GetCategory(1, title);
            var sourceCategoryDto = GetCategoryDTO();
            sourceCategoryDto.Title = title;
            sourceCategoryDto.ImageId = sourceCategory.ImageId;
            sourceCategoryDto.Id = sourceCategory.Id;
            this.SetupMapper(sourceCategory, sourceCategoryDto);

            var handler = new UpdateCategoryHandler(
                this._mockRepository.Object,
                this.mockMapper.Object,
                this.mockLogger.Object,
                this.mockLocalizerFailedToUpdate.Object,
                this.mockLocalizerConvertNull.Object,
                this.mockLocalizerCannotFindSharedResource.Object);

            this._mockRepository
                .Setup(p => p.SourceCategoryRepository.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<DAL.Entities.Sources.SourceLinkCategory, bool>>>(),
                    It.IsAny<Func<IQueryable<DAL.Entities.Sources.SourceLinkCategory>,
                    IIncludableQueryable<DAL.Entities.Sources.SourceLinkCategory, object>>>()))
                .ReturnsAsync(sourceCategory);

            this._mockRepository
                .Setup(x => x.ImageRepository.GetFirstOrDefaultAsync(
                   It.IsAny<Expression<Func<Image, bool>>>(),
                   It.IsAny<Func<IQueryable<Image>, IIncludableQueryable<Image, object>>>()))
                .ReturnsAsync(new Image());

            var expectedError = "Failed to update category";
            this.mockLocalizerFailedToUpdate.Setup(x => x["FailedToUpdateCategory"])
            .Returns(new LocalizedString("FailedToUpdateCategory", expectedError));

            // Act
            var result = await handler.Handle(new UpdateCategoryCommand(sourceCategoryDto), CancellationToken.None);

            // Assert
            Assert.True(result.IsFailed);
            Assert.Equal(expectedError, result.Errors[0].Message);
        }

        [Theory]
        [InlineData(1)]
        public async Task ShouldReturnSuccessfully_TypeIsCorrect(int returnNumber)
        {
            // Arrange
            this.SetupCreateRepository(returnNumber);
            string title = "Tested";
            var sourceCategory = GetCategory(1, title);
            var sourceCategoryDto = GetCategoryDTO();
            sourceCategoryDto.Title = title;
            sourceCategoryDto.Id = sourceCategory.Id;
            sourceCategoryDto.ImageId = sourceCategory.ImageId;
            this.SetupMapper(sourceCategory, sourceCategoryDto);

            var handler = new UpdateCategoryHandler(
                this._mockRepository.Object,
                this.mockMapper.Object,
                this.mockLogger.Object,
                this.mockLocalizerFailedToUpdate.Object,
                this.mockLocalizerConvertNull.Object,
                this.mockLocalizerCannotFindSharedResource.Object);

            this._mockRepository
                .Setup(p => p.SourceCategoryRepository.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<DAL.Entities.Sources.SourceLinkCategory, bool>>>(),
                    It.IsAny<Func<IQueryable<DAL.Entities.Sources.SourceLinkCategory>,
                    IIncludableQueryable<DAL.Entities.Sources.SourceLinkCategory, object>>>()))
                .ReturnsAsync(sourceCategory);

            this._mockRepository
                .Setup(x => x.ImageRepository.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<Image, bool>>>(),
                    It.IsAny<Func<IQueryable<Image>, IIncludableQueryable<Image, object>>>()))
                .ReturnsAsync(new Image());

            // Act
            var result = await handler.Handle(new UpdateCategoryCommand(sourceCategoryDto), CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task ShouldReturnFail_ImageIdIsZero()
        {
            // Arrange
            string title = "Tested";
            var sourceCategory = GetCategory(0, title);
            var sourceCategoryDto = GetCategoryDTO();
            sourceCategoryDto.Title = title;
            sourceCategoryDto.Id = sourceCategory.Id;
            this.SetupMapper(sourceCategory, sourceCategoryDto);

            var handler = new UpdateCategoryHandler(
                this._mockRepository.Object,
                this.mockMapper.Object,
                this.mockLogger.Object,
                this.mockLocalizerFailedToUpdate.Object,
                this.mockLocalizerConvertNull.Object,
                this.mockLocalizerCannotFindSharedResource.Object);

            this._mockRepository
                .Setup(p => p.SourceCategoryRepository.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<DAL.Entities.Sources.SourceLinkCategory, bool>>>(),
                    It.IsAny<Func<IQueryable<DAL.Entities.Sources.SourceLinkCategory>,
                    IIncludableQueryable<DAL.Entities.Sources.SourceLinkCategory, object>>>()))
                .ReturnsAsync(sourceCategory);

            this.SetupImageRepository();

            string expectedErrorMessage = "Cannot find an image with corresponding id: 0";

            // Act
            var result = await handler.Handle(new UpdateCategoryCommand(sourceCategoryDto), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.True(result.IsFailed),
                () => Assert.Equal(expectedErrorMessage, result.Errors[0].Message));
        }

        [Fact]
        public async Task UpdateCategoryHandler_TitleIsNotValid_With_Text_More_Then_23_ReturnsValidationError()
        {
            // Arrange
            string? title = "This title is definitely more than 23 characters long and contains whitespace";
            var categoryDto = new UpdateSourceLinkCategoryDTO
            {
                Id = 1,
                Title = title,
                ImageId = 1,
            };

            var category = GetCategory(1, title);

            var command = new UpdateCategoryCommand(categoryDto);

            var handler = new UpdateCategoryHandler(
                       this._mockRepository.Object,
                       this.mockMapper.Object,
                       this.mockLogger.Object,
                       this.mockLocalizerFailedToUpdate.Object,
                       this.mockLocalizerConvertNull.Object,
                       this.mockLocalizerCannotFindSharedResource.Object);

            this._mockRepository
                .Setup(p => p.SourceCategoryRepository.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<DAL.Entities.Sources.SourceLinkCategory, bool>>>(),
                    It.IsAny<Func<IQueryable<DAL.Entities.Sources.SourceLinkCategory>,
                    IIncludableQueryable<DAL.Entities.Sources.SourceLinkCategory, object>>>()))
                .ReturnsAsync(category);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Title cannot be longer than 23 characters.", result.Errors[0].Message);
        }

        [Fact]
        public async Task UpdateCategoryHandler_TitleIsNotValid_With_WhiteSpace_ReturnsValidationError()
        {
            // Arrange
            string? title = " ";
            var categoryDto = new UpdateSourceLinkCategoryDTO
            {
                Id = 1,
                Title = title,
                ImageId = 1,
            };

            var category = GetCategory(1, title);

            var command = new UpdateCategoryCommand(categoryDto);

            var handler = new UpdateCategoryHandler(
                       this._mockRepository.Object,
                       this.mockMapper.Object,
                       this.mockLogger.Object,
                       this.mockLocalizerFailedToUpdate.Object,
                       this.mockLocalizerConvertNull.Object,
                       this.mockLocalizerCannotFindSharedResource.Object);

            this._mockRepository
                .Setup(p => p.SourceCategoryRepository.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<DAL.Entities.Sources.SourceLinkCategory, bool>>>(),
                    It.IsAny<Func<IQueryable<DAL.Entities.Sources.SourceLinkCategory>,
                    IIncludableQueryable<DAL.Entities.Sources.SourceLinkCategory, object>>>()))
               .ReturnsAsync(category);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Title cannot be empty.", result.Errors[0].Message);
        }

        [Fact]
        public async Task UpdateCategoryHandler_TitleAlreadyExists_ReturnsErrorMessage()
        {
            // Arrange
            string title = "ExistingTitle";
            var existingCategory = GetCategory(1, title);
            var newCategoryDto = GetCategoryDTO();
            newCategoryDto.Title = title;
            newCategoryDto.Id = 2;

            this.SetupMapper(existingCategory, newCategoryDto);

            var handler = new UpdateCategoryHandler(
                this._mockRepository.Object,
                this.mockMapper.Object,
                this.mockLogger.Object,
                this.mockLocalizerFailedToUpdate.Object,
                this.mockLocalizerConvertNull.Object,
                this.mockLocalizerCannotFindSharedResource.Object);

            this._mockRepository
                .Setup(p => p.SourceCategoryRepository.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<DAL.Entities.Sources.SourceLinkCategory, bool>>>(),
                    It.IsAny<Func<IQueryable<DAL.Entities.Sources.SourceLinkCategory>,
                    IIncludableQueryable<DAL.Entities.Sources.SourceLinkCategory, object>>>()))
                .ReturnsAsync(existingCategory);

            var expectedError = $"Title: {newCategoryDto.Title} already exists";

            // Act
            var result = await handler.Handle(new UpdateCategoryCommand(newCategoryDto), CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(expectedError, result.Errors[0].Message);
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

        private static UpdateSourceLinkCategoryDTO GetCategoryDTO()
        {
            return new UpdateSourceLinkCategoryDTO();
        }

        private void SetupCreateRepository(int returnNumber)
        {
            this._mockRepository.Setup(x => x.SourceCategoryRepository.Create(GetCategory(1, null)));
            this._mockRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(returnNumber);
        }

        private void SetupMapper(DAL.Entities.Sources.SourceLinkCategory testCategory, UpdateSourceLinkCategoryDTO testCategoryDTO)
        {
            this.mockMapper.Setup(x => x.Map<DAL.Entities.Sources.SourceLinkCategory>(It.IsAny<UpdateSourceLinkCategoryDTO>()))
                .Returns(testCategory);
            this.mockMapper.Setup(x => x.Map<UpdateSourceLinkCategoryDTO>(It.IsAny<DAL.Entities.Sources.SourceLinkCategory>()))
                .Returns(testCategoryDTO);
        }

        private void SetupImageRepository()
        {
            this._mockRepository
                .Setup(x => x.ImageRepository.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<Image, bool>>>(),
                    It.IsAny<Func<IQueryable<Image>,
                    IIncludableQueryable<Image, object>>>()))
                .ReturnsAsync((Image?)null);
        }
    }
}
