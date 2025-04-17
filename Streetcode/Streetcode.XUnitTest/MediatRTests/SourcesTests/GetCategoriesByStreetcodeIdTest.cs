using System.Linq.Expressions;
using AutoMapper;
using FluentResults;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using MockQueryable.Moq;
using Moq;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.DTO.Sources;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Sources.SourceLink.GetCategoriesByStreetcodeId;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Sources;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Enums;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.SourcesTests
{
    public class GetCategoriesByStreetcodeIdTest
    {
        private readonly Mock<IRepositoryWrapper> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IBlobService> _blobService;
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly MockCannotFindLocalizer _mockLocalizerCannotFind;

        public GetCategoriesByStreetcodeIdTest()
        {
            _mockRepository = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _blobService = new Mock<IBlobService>();
            _mockLogger = new Mock<ILoggerService>();
            _mockLocalizerCannotFind = new MockCannotFindLocalizer();
        }

        [Theory]
        [InlineData(1)]
        public async Task Handler_CategoriesExistsAndUserHasAccess_ReturnSuccesfully(int id)
        {
            // Arrange
            SetupRepositoryMock(GetSourceLinkCategories(), GetStreetcodeList());

            _mockMapper.Setup(x => x.Map<IEnumerable<SourceLinkCategoryDTO>>(It.IsAny<IEnumerable<SourceLinkCategory>>()))
                .Returns(GetSourceDTOs());

            var handler = new GetCategoriesByStreetcodeIdHandler(
                _mockRepository.Object,
                _mockMapper.Object,
                _blobService.Object,
                _mockLogger.Object,
                _mockLocalizerCannotFind);

            // Act
            var result = await handler.Handle(new GetCategoriesByStreetcodeIdQuery(id, UserRole.User), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.NotEmpty(result.Value),
                () => Assert.IsAssignableFrom<IEnumerable<SourceLinkCategoryDTO>>(result.ValueOrDefault));
        }

        [Theory]
        [InlineData(1)]
        public async Task Handler_CategoriesNotExistsAndUserHasAccess_ReturnEmptyArray(int id)
        {
            // Arrange
            SetupRepositoryMock(GetSourceLinkCategoriesNotExists(), GetStreetcodeList());

            var handler = new GetCategoriesByStreetcodeIdHandler(
                _mockRepository.Object,
                _mockMapper.Object,
                _blobService.Object,
                _mockLogger.Object,
                _mockLocalizerCannotFind);

            // Act
            var result = await handler.Handle(new GetCategoriesByStreetcodeIdQuery(id, UserRole.User), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.IsType<Result<IEnumerable<SourceLinkCategoryDTO>>>(result),
                () => Assert.IsAssignableFrom<IEnumerable<SourceLinkCategoryDTO>>(result.Value),
                () => Assert.Empty(result.Value));
        }

        [Theory]
        [InlineData(1)]
        public async Task Handler_CategoriesExistsButUserDoesNotHaveAccess_ReturnSuccesfully(int id)
        {
            // Arrange
            SetupRepositoryMock(GetSourceLinkCategories(), new List<StreetcodeContent>());

            var expectedError = _mockLocalizerCannotFind["CannotFindAnyStreetcodeWithCorrespondingId", id].Value;

            _mockMapper.Setup(x => x.Map<IEnumerable<SourceLinkCategoryDTO>>(It.IsAny<IEnumerable<SourceLinkCategory>>()))
                .Returns(GetSourceDTOs());

            var handler = new GetCategoriesByStreetcodeIdHandler(
                _mockRepository.Object,
                _mockMapper.Object,
                _blobService.Object,
                _mockLogger.Object,
                _mockLocalizerCannotFind);

            // Act
            var result = await handler.Handle(new GetCategoriesByStreetcodeIdQuery(id, UserRole.User), CancellationToken.None);

            // Assert
            Assert.Equal(expectedError, result.Errors.Single().Message);
        }

        private void SetupRepositoryMock(List<SourceLinkCategory> sourceLinkCategories, List<StreetcodeContent> streetcodeListUserCanAccess)
        {
            _mockRepository.Setup(x => x.SourceCategoryRepository
                    .GetAllAsync(
                        It.IsAny<Expression<Func<SourceLinkCategory, bool>>>(),
                        It.IsAny<Func<IQueryable<SourceLinkCategory>,
                            IIncludableQueryable<SourceLinkCategory, object>>>()))
                .ReturnsAsync(sourceLinkCategories);

            _mockRepository.Setup(repo => repo.StreetcodeRepository
                    .FindAll(
                        It.IsAny<Expression<Func<StreetcodeContent, bool>>>(),
                        It.IsAny<Func<IQueryable<StreetcodeContent>,
                            IIncludableQueryable<StreetcodeContent, object>>>()))
                .Returns(streetcodeListUserCanAccess.AsQueryable().BuildMockDbSet().Object);
        }

        private static List<SourceLinkCategory> GetSourceLinkCategories()
        {
            return new List<SourceLinkCategory>()
            {
                new SourceLinkCategory()
                {
                    Id = 1,
                    Image = new DAL.Entities.Media.Images.Image(),
                },
                new SourceLinkCategory()
                {
                    Id = 2,
                    Image = new DAL.Entities.Media.Images.Image(),
                },
            };
        }

        private static List<SourceLinkCategoryDTO> GetSourceDTOs()
        {
            return new List<SourceLinkCategoryDTO>()
            {
                new SourceLinkCategoryDTO()
                {
                    Id = 1,
                    Image = new BLL.DTO.Media.Images.ImageDTO(),
                },
                new SourceLinkCategoryDTO()
                {
                    Id = 2,
                    Image = new BLL.DTO.Media.Images.ImageDTO(),
                },
            };
        }

        private List<SourceLinkCategory> GetSourceLinkCategoriesNotExists()
        {
            return new List<SourceLinkCategory>();
        }

        private static List<StreetcodeContent> GetStreetcodeList()
        {
            return new List<StreetcodeContent>
            {
                new StreetcodeContent
                {
                    Id = 1,
                },
            };
        }
    }
}
