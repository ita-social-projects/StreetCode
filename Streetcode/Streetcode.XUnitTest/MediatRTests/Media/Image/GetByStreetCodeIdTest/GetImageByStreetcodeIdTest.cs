using System.Linq.Expressions;
using AutoMapper;
using FluentResults;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using MockQueryable.Moq;
using Moq;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.Interfaces.Cache;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Media.Image.GetByStreetcodeId;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Enums;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Media.Images
{
    public class GetImageByStreetcodeIdTest
    {
        private readonly Mock<IRepositoryWrapper> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IBlobService> _blobService;
        private readonly MockCannotFindLocalizer _mockLocalizer;
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly Mock<ICacheService> _mockCache;

        public GetImageByStreetcodeIdTest()
        {
            _mockRepo = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _blobService = new Mock<IBlobService>();
            _mockLogger = new Mock<ILoggerService>();
            _mockLocalizer = new MockCannotFindLocalizer();
            _mockCache = new Mock<ICacheService>();
            _mockCache
                .Setup(c => c.GetOrSetAsync(It.IsAny<string>(), It.IsAny<Func<Task<Result<IEnumerable<ImageDTO>>>>>(), It.IsAny<TimeSpan>()))
                .Returns<string, Func<Task<Result<IEnumerable<ImageDTO>>>>, TimeSpan>((key, func, timeSpan) =>
                {
                    return func();
                });
        }

        [Theory]
        [InlineData(1)]

        public async Task Handle_ReturnsImage(int streetcodeId)
        {
            // Arrange
            MockRepositoryAndMapper(GetImagesList(), GetImagesDTOList(), GetStreetcodesUserHaveAccessTo());
            var handler = new GetImageByStreetcodeIdHandler(_mockRepo.Object, _mockMapper.Object, _blobService.Object, _mockLogger.Object, _mockLocalizer, _mockCache.Object);

            // Act
            var result = await handler.Handle(new GetImageByStreetcodeIdQuery(streetcodeId, UserRole.User), CancellationToken.None);

            // Assert
            Assert.Equal(streetcodeId, result.Value.First().Id);
        }

        [Theory]
        [InlineData(1)]

        public async Task Handle_ReturnsType(int streetcodeId)
        {
            // Arrange
            MockRepositoryAndMapper(GetImagesList(), GetImagesDTOList(), GetStreetcodesUserHaveAccessTo());
            var handler = new GetImageByStreetcodeIdHandler(_mockRepo.Object, _mockMapper.Object, _blobService.Object, _mockLogger.Object, _mockLocalizer, _mockCache.Object);

            // Act
            var result = await handler.Handle(new GetImageByStreetcodeIdQuery(streetcodeId, UserRole.User), default);

            // Assert
            Assert.IsType<Result<IEnumerable<ImageDTO>>>(result);
        }

        [Theory]
        [InlineData(-1)]
        public async Task Handle_WithNonExistentId_ReturnsError(int streetcodeId)
        {
            // Arrange
            MockRepositoryAndMapper(new List<DAL.Entities.Media.Images.Image>(), new List<ImageDTO>(), GetStreetcodesUserHaveAccessTo());

            var expectedError = _mockLocalizer["CannotFindAnImageWithTheCorrespondingStreetcodeId", streetcodeId].Value;

            var handler = new GetImageByStreetcodeIdHandler(_mockRepo.Object, _mockMapper.Object, _blobService.Object, _mockLogger.Object, _mockLocalizer, _mockCache.Object);

            // Act
            var result = await handler.Handle(new GetImageByStreetcodeIdQuery(streetcodeId, UserRole.User), CancellationToken.None);

            // Assert
            Assert.Equal(expectedError, result.Errors[0].Message);
        }

        [Theory]
        [InlineData(-1)]
        public async Task Handle_UserDoesNotHaveAccess_ReturnsErrorNoStreetcode(int streetcodeId)
        {
            // Arrange
            MockRepositoryAndMapper(GetImagesList(), GetImagesDTOList(), new List<StreetcodeContent>());

            var expectedError = _mockLocalizer["CannotFindAnyStreetcodeWithCorrespondingId", streetcodeId].Value;

            var handler = new GetImageByStreetcodeIdHandler(_mockRepo.Object, _mockMapper.Object, _blobService.Object, _mockLogger.Object, _mockLocalizer, _mockCache.Object);

            // Act
            var result = await handler.Handle(new GetImageByStreetcodeIdQuery(streetcodeId, UserRole.User), CancellationToken.None);

            // Assert
            Assert.Equal(expectedError, result.Errors[0].Message);
        }

        private List<DAL.Entities.Media.Images.Image> GetImagesList()
        {
            return new List<DAL.Entities.Media.Images.Image>()
            {
                new DAL.Entities.Media.Images.Image()
                {
                    Id = 1,
                },
                new DAL.Entities.Media.Images.Image()
                {
                    Id = 2,
                },
            };
        }

        private List<ImageDTO> GetImagesDTOList()
        {
            return new List<ImageDTO>()
            {
                new ImageDTO
                {
                    Id = 1,
                },
                new ImageDTO
                {
                    Id = 2,
                },
            };
        }

        private List<StreetcodeContent> GetStreetcodesUserHaveAccessTo()
        {
            return new List<StreetcodeContent>()
            {
                new StreetcodeContent
                {
                    Id = 1,
                },
            };
        }

        private void MockRepositoryAndMapper(List<DAL.Entities.Media.Images.Image> imageList, List<ImageDTO> imageListDTO, List<StreetcodeContent> streetcodeListUserCanAccess)
        {
            _mockRepo
                .Setup(r => r.ImageRepository.GetAllAsync(
                    It.IsAny<Expression<Func<DAL.Entities.Media.Images.Image, bool>>>(),
                    It.IsAny<Func<IQueryable<DAL.Entities.Media.Images.Image>,
                    IIncludableQueryable<DAL.Entities.Media.Images.Image, object>>>()))
                .ReturnsAsync(imageList);

            _mockRepo.Setup(repo => repo.StreetcodeRepository
                    .FindAll(
                        It.IsAny<Expression<Func<StreetcodeContent, bool>>>(),
                        It.IsAny<Func<IQueryable<StreetcodeContent>,
                            IIncludableQueryable<StreetcodeContent, object>>>()))
                .Returns(streetcodeListUserCanAccess.AsQueryable().BuildMockDbSet().Object);

            _mockMapper
                .Setup(x => x.Map<IEnumerable<ImageDTO>>(It.IsAny<IEnumerable<object>>()))
                .Returns(imageListDTO);
        }
    }
}