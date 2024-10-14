using AutoMapper;
using Moq;
using Streetcode.BLL.DTO.Sources;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.MediatR.Sources.SourceLink.Create;
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
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.MediatR.Sources.SourceLinkCategory.Create;
using Streetcode.BLL.MediatR.Team.Create;

namespace Streetcode.XUnitTest.MediatRTests.SourcesTests
{
    public class CreateCategoryTests
    {
        private Mock<IRepositoryWrapper> _mockRepository;
        private Mock<IMapper> _mockMapper;
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly Mock<IStringLocalizer<CannotConvertNullSharedResource>> _mockLocalizerConvertNull;

        public CreateCategoryTests()
        {
            _mockRepository = new();
            _mockMapper = new();
            _mockLogger = new Mock<ILoggerService>();
            _mockLocalizerConvertNull = new Mock<IStringLocalizer<CannotConvertNullSharedResource>>();
        }

        [Fact]
        public async Task CreateCategoryHandler_SaveChangesFailed_ReturnsErrorMessage()
        {
            // Arrange
            SetupCreateRepository(-1);
            var categoryDto = GetCategoryDTO();
            SetupMapper(GetCategory(1, "Title"), categoryDto);

            var handler = new CreateCategoryHandler(
                _mockRepository.Object,
                _mockMapper.Object,
                _mockLogger.Object,
                _mockLocalizerConvertNull.Object);

            _mockRepository.Setup(x => x.ImageRepository.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<Image, bool>>>(),
                    It.IsAny<Func<IQueryable<Image>, IIncludableQueryable<Image, object>>>()
                )).ReturnsAsync(new Image());

            _mockRepository.Setup(p => p.SourceCategoryRepository
         .GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DAL.Entities.Sources.SourceLinkCategory, bool>>>(),
          It.IsAny<Func<IQueryable<DAL.Entities.Sources.SourceLinkCategory>,
          IIncludableQueryable<DAL.Entities.Sources.SourceLinkCategory, object>>>()))
             .ReturnsAsync(null as DAL.Entities.Sources.SourceLinkCategory);

            var expectedError = "Failed to create category";

            // Act
            var result = await handler.Handle(new CreateCategoryCommand(categoryDto), CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(expectedError, result.Errors.First().Message);
        }

        [Fact]
        public async Task CreateCategoryHandler_SuccessfulCreation_ReturnsSuccess()
        {
            // Arrange
            string title = "NewTitle";
            var newCategory = GetCategory(1, title);
            var newCategoryDto = GetCategoryDTO();
            newCategoryDto.Title = title;

            SetupCreateRepository(1); // Simulate successful SaveChangesAsync
            SetupMapper(newCategory, newCategoryDto);

            var handler = new CreateCategoryHandler(
                _mockRepository.Object,
                _mockMapper.Object,
                _mockLogger.Object,
                _mockLocalizerConvertNull.Object);

            _mockRepository.Setup(x => x.ImageRepository.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<Image, bool>>>(),
                    It.IsAny<Func<IQueryable<Image>, IIncludableQueryable<Image, object>>>()
                )).ReturnsAsync(new Image());

            _mockRepository.Setup(p => p.SourceCategoryRepository
      .GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DAL.Entities.Sources.SourceLinkCategory, bool>>>(),
       It.IsAny<Func<IQueryable<DAL.Entities.Sources.SourceLinkCategory>,
       IIncludableQueryable<DAL.Entities.Sources.SourceLinkCategory, object>>>()))
          .ReturnsAsync(null as DAL.Entities.Sources.SourceLinkCategory);

            // Act
            var result = await handler.Handle(new CreateCategoryCommand(newCategoryDto), CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
        }

        private void SetupCreateRepository(int returnNumber)
        {
            _mockRepository.Setup(x => x.SourceCategoryRepository.Create(It.IsAny<DAL.Entities.Sources.SourceLinkCategory>()));
            _mockRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(returnNumber);
        }

        private void SetupMapper(DAL.Entities.Sources.SourceLinkCategory testCategory, SourceLinkCategoryCreateDTO testCategoryDTO)
        {
            _mockMapper.Setup(x => x.Map<DAL.Entities.Sources.SourceLinkCategory>(It.IsAny<SourceLinkCategoryCreateDTO>()))
                .Returns(testCategory);
            _mockMapper.Setup(x => x.Map<SourceLinkCategoryCreateDTO>(It.IsAny<DAL.Entities.Sources.SourceLinkCategory>()))
                .Returns(testCategoryDTO);
        }

        private void SetupMapperWithNullCategory()
        {
            _mockMapper.Setup(x => x.Map<DAL.Entities.Sources.SourceLinkCategory>(It.IsAny<CreateSourceLinkCategoryDTO>()))
                .Returns(GetCategoryWithNotExistId());
        }

        private void SetupImageRepository()
        {
            _mockRepository.Setup(x => x.ImageRepository.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<Image, bool>>>(),
                    It.IsAny<Func<IQueryable<Image>, IIncludableQueryable<Image, object>>>()
                )).ReturnsAsync((Image)null);
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

        private static DAL.Entities.Sources.SourceLinkCategory GetCategoryWithNotExistId() => null;

        private static SourceLinkCategoryCreateDTO GetCategoryDTOWithNotExistId() => null;
    }
}
