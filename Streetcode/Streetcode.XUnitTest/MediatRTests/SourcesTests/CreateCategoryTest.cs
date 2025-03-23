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
        private readonly Mock<IRepositoryWrapper> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly Mock<IStringLocalizer<CannotConvertNullSharedResource>> _mockLocalizerConvertNull;
        private readonly Mock<IStringLocalizer<FailedToCreateSharedResource>> _mockLocalizerFailedToCreate;

        public CreateCategoryTest()
        {
            _mockRepository = new ();
            _mockMapper = new ();
            _mockLogger = new Mock<ILoggerService>();
            _mockLocalizerConvertNull = new Mock<IStringLocalizer<CannotConvertNullSharedResource>>();
            _mockLocalizerFailedToCreate = new Mock<IStringLocalizer<FailedToCreateSharedResource>>();
            _mockLocalizerFailedToCreate.Setup(x => x["FailedToCreateCategory"]).Returns(new LocalizedString("FailedToCreateCategory", "FailedToCreateCategory"));
        }

        [Fact]
        public async Task CreateCategoryHandler_SaveChangesFailed_ReturnsErrorMessage()
        {
            // Arrange
            SetupCreateRepository(-1);
            var categoryDto = GetCategoryDto();
            SetupMapper(GetCategory(1, "Title"), categoryDto);

            var handler = new CreateCategoryHandler(
                _mockRepository.Object,
                _mockMapper.Object,
                _mockLogger.Object,
                _mockLocalizerConvertNull.Object,
                _mockLocalizerFailedToCreate.Object);

            _mockRepository.Setup(x => x.ImageRepository.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<Image, bool>>>(),
                    It.IsAny<Func<IQueryable<Image>, IIncludableQueryable<Image, object>>>()))
                .ReturnsAsync(new Image());

            _mockRepository.Setup(p => p.SourceCategoryRepository
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
            var newCategoryDto = GetCategoryDto();
            newCategoryDto.Title = title;

            SetupCreateRepository(1); // Simulate successful SaveChangesAsync
            SetupMapper(newCategory, newCategoryDto);

            var handler = new CreateCategoryHandler(
                _mockRepository.Object,
                _mockMapper.Object,
                _mockLogger.Object,
                _mockLocalizerConvertNull.Object,
                _mockLocalizerFailedToCreate.Object);

            _mockRepository.Setup(x => x.ImageRepository.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<Image, bool>>>(),
                    It.IsAny<Func<IQueryable<Image>, IIncludableQueryable<Image, object>>>()))
                .ReturnsAsync(new Image());

            _mockRepository.Setup(p => p.SourceCategoryRepository
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

        private static SourceLinkCategoryCreateDTO GetCategoryDto()
        {
            return new SourceLinkCategoryCreateDTO()
            {
                ImageId = 1,
                Title = "Title",
            };
        }

        private void SetupCreateRepository(int returnNumber)
        {
            _mockRepository.Setup(x => x.SourceCategoryRepository.Create(It.IsAny<DAL.Entities.Sources.SourceLinkCategory>()));
            _mockRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(returnNumber);
        }

        private void SetupMapper(DAL.Entities.Sources.SourceLinkCategory testCategory, SourceLinkCategoryCreateDTO testCategoryDto)
        {
            _mockMapper.Setup(x => x.Map<DAL.Entities.Sources.SourceLinkCategory>(It.IsAny<SourceLinkCategoryCreateDTO>()))
                .Returns(testCategory);
            _mockMapper.Setup(x => x.Map<SourceLinkCategoryCreateDTO>(It.IsAny<DAL.Entities.Sources.SourceLinkCategory>()))
                .Returns(testCategoryDto);
        }
    }
}
