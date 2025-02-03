using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Sources;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Sources.SourceLink.Create;
using Streetcode.BLL.MediatR.Sources.SourceLinkCategory.Create;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.SourcesTests
{
    public class CreateCategoryTest
    {
        private readonly Mock<IRepositoryWrapper> mockRepository;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<ILoggerService> mockLogger;
        private readonly Mock<IStringLocalizer<CannotConvertNullSharedResource>> mockLocalizerConvertNull;
        private readonly Mock<IStringLocalizer<FailedToCreateSharedResource>> mockLocalizerFailedToCreate;

        public CreateCategoryTest()
        {
            this.mockRepository = new ();
            this.mockMapper = new ();
            this.mockLogger = new Mock<ILoggerService>();
            this.mockLocalizerConvertNull = new Mock<IStringLocalizer<CannotConvertNullSharedResource>>();
            this.mockLocalizerFailedToCreate = new Mock<IStringLocalizer<FailedToCreateSharedResource>>();
            this.mockLocalizerFailedToCreate.Setup(x => x["FailedToCreateCategory"]).Returns(new LocalizedString("FailedToCreateCategory", "FailedToCreateCategory"));
        }

        [Fact]
        public async Task CreateCategoryHandler_SaveChangesFailed_ReturnsErrorMessage()
        {
            // Arrange
            this.SetupCreateRepository(-1);
            var categoryDto = GetCategoryDTO();
            this.SetupMapper(GetCategory(1, "Title"), categoryDto);

            var handler = new CreateCategoryHandler(
                this.mockRepository.Object,
                this.mockMapper.Object,
                this.mockLogger.Object,
                this.mockLocalizerConvertNull.Object,
                this.mockLocalizerFailedToCreate.Object);

            this.mockRepository.Setup(x => x.ImageRepository.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<Image, bool>>>(),
                    It.IsAny<Func<IQueryable<Image>, IIncludableQueryable<Image, object>>>()))
                .ReturnsAsync(new Image());

            this.mockRepository.Setup(p => p.SourceCategoryRepository
                .GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<DAL.Entities.Sources.SourceLinkCategory, bool>>>(),
                    It.IsAny<Func<IQueryable<DAL.Entities.Sources.SourceLinkCategory>,
                    IIncludableQueryable<DAL.Entities.Sources.SourceLinkCategory, object>>>()))
                .ReturnsAsync(null as DAL.Entities.Sources.SourceLinkCategory);

            var expectedError = "FailedToCreateCategory";

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
                this.mockRepository.Object,
                this.mockMapper.Object,
                this.mockLogger.Object,
                this.mockLocalizerConvertNull.Object,
                this.mockLocalizerFailedToCreate.Object);

            this.mockRepository.Setup(x => x.ImageRepository.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<Image, bool>>>(),
                    It.IsAny<Func<IQueryable<Image>, IIncludableQueryable<Image, object>>>()))
                .ReturnsAsync(new Image());

            this.mockRepository.Setup(p => p.SourceCategoryRepository
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

        private static SourceLinkCategoryCreateDto GetCategoryDTO()
        {
            return new SourceLinkCategoryCreateDto()
            {
                ImageId = 1,
                Title = "Title",
            };
        }

        private void SetupCreateRepository(int returnNumber)
        {
            this.mockRepository.Setup(x => x.SourceCategoryRepository.Create(It.IsAny<DAL.Entities.Sources.SourceLinkCategory>()));
            this.mockRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(returnNumber);
        }

        private void SetupMapper(DAL.Entities.Sources.SourceLinkCategory testCategory, SourceLinkCategoryCreateDto testCategoryDTO)
        {
            this.mockMapper.Setup(x => x.Map<DAL.Entities.Sources.SourceLinkCategory>(It.IsAny<SourceLinkCategoryCreateDto>()))
                .Returns(testCategory);
            this.mockMapper.Setup(x => x.Map<SourceLinkCategoryCreateDto>(It.IsAny<DAL.Entities.Sources.SourceLinkCategory>()))
                .Returns(testCategoryDTO);
        }
    }
}
