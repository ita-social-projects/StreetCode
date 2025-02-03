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

            this.mockMapper.Setup(m => m.Map<DAL.Entities.Sources.SourceLinkCategory?>(It.IsAny<SourceLinkCreateUpdateCategoryDto>()))
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

        private static DAL.Entities.Sources.SourceLinkCategory GetCategory(int imageId = 0, string? title = null)
        {
            return new DAL.Entities.Sources.SourceLinkCategory()
            {
                Id = 1,
                ImageId = imageId,
                Title = title,
            };
        }

        private static UpdateSourceLinkCategoryDto GetCategoryDTO()
        {
            return new UpdateSourceLinkCategoryDto();
        }

        private void SetupCreateRepository(int returnNumber)
        {
            this._mockRepository.Setup(x => x.SourceCategoryRepository.Create(GetCategory(1, null)));
            this._mockRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(returnNumber);
        }

        private void SetupMapper(DAL.Entities.Sources.SourceLinkCategory testCategory, UpdateSourceLinkCategoryDto testCategoryDTO)
        {
            this.mockMapper.Setup(x => x.Map<DAL.Entities.Sources.SourceLinkCategory>(It.IsAny<UpdateSourceLinkCategoryDto>()))
                .Returns(testCategory);
            this.mockMapper.Setup(x => x.Map<UpdateSourceLinkCategoryDto>(It.IsAny<DAL.Entities.Sources.SourceLinkCategory>()))
                .Returns(testCategoryDTO);
        }
    }
}
