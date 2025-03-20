using System.Linq.Expressions;
using AutoMapper;
using FluentResults;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using MockQueryable.Moq;
using Moq;
using Streetcode.BLL.DTO.AdditionalContent.Tag;
using Streetcode.BLL.DTO.Media.Art;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Media.Art.GetByStreetcodeId;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Enums;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Media.Arts
{
    public class GetArtByStreetcodeIdTest
    {
        private readonly Mock<IRepositoryWrapper> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IBlobService> _blobService;
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly MockCannotFindLocalizer _mockLocalizer;

        public GetArtByStreetcodeIdTest()
        {
            _mockRepo = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _blobService = new Mock<IBlobService>();
            _mockLogger = new Mock<ILoggerService>();
            _mockLocalizer = new MockCannotFindLocalizer();
        }

        [Theory]
        [InlineData(1)]

        public async Task Handle_ReturnsArt(int streetcodeId)
        {
            // Arrange
            MockRepositoryAndMapper(GetArtsList(), GetArtsDTOList(), new List<StreetcodeContent>() { new StreetcodeContent() { Id = streetcodeId } });
            var handler = new GetArtsByStreetcodeIdHandler(_mockRepo.Object, _mockMapper.Object, _blobService.Object, _mockLogger.Object, _mockLocalizer);

            // Act
            var result = await handler.Handle(new GetArtsByStreetcodeIdQuery(streetcodeId, UserRole.User), CancellationToken.None);

            // Assert
            Assert.Equal(streetcodeId, result.Value.First().Id);
        }

        [Theory]
        [InlineData(1)]

        public async Task Handle_ReturnsEmptyArray(int streetcodeId)
        {
            // Arrange
            MockRepositoryAndMapper(new List<Art>(), new List<ArtDTO>(), new List<StreetcodeContent>() { new StreetcodeContent() { Id = streetcodeId } });
            var handler = new GetArtsByStreetcodeIdHandler(_mockRepo.Object, _mockMapper.Object, _blobService.Object, _mockLogger.Object, _mockLocalizer);

            // Act
            var result = await handler.Handle(new GetArtsByStreetcodeIdQuery(streetcodeId, UserRole.User), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.IsType<Result<IEnumerable<ArtDTO>>>(result),
                () => Assert.IsAssignableFrom<IEnumerable<ArtDTO>>(result.Value),
                () => Assert.Empty(result.Value));
        }

        [Theory]
        [InlineData(1)]

        public async Task Handle_ReturnsType(int streetcodeId)
        {
            // Arrange
            MockRepositoryAndMapper(GetArtsList(), GetArtsDTOList(), new List<StreetcodeContent>() { new StreetcodeContent() { Id = streetcodeId } });
            var handler = new GetArtsByStreetcodeIdHandler(_mockRepo.Object, _mockMapper.Object, _blobService.Object, _mockLogger.Object, _mockLocalizer);

            // Act
            var result = await handler.Handle(new GetArtsByStreetcodeIdQuery(streetcodeId, UserRole.User), CancellationToken.None);

            // Assert
            Assert.IsType<Result<IEnumerable<ArtDTO>>>(result);
        }

        [Theory]
        [InlineData(1)]

        public async Task Handle_UserDoesNotHaveAccess_ReturnsErrorNoStreetcode(int streetcodeId)
        {
            // Arrange
            MockRepositoryAndMapper(GetArtsList(), GetArtsDTOList(), new List<StreetcodeContent>());

            var expectedError = _mockLocalizer["CannotFindAnyArtWithCorrespondingStreetcodeId", streetcodeId].Value;

            var handler = new GetArtsByStreetcodeIdHandler(_mockRepo.Object, _mockMapper.Object, _blobService.Object, _mockLogger.Object, _mockLocalizer);

            // Act
            var result = await handler.Handle(new GetArtsByStreetcodeIdQuery(streetcodeId, UserRole.User), CancellationToken.None);

            // Assert
            Assert.Equal(expectedError, result.Errors.Single().Message);
        }

        private List<Art> GetArtsList()
        {
            return new List<Art>()
            {
                new Art()
                {
                    Id = 1,
                    Image = new DAL.Entities.Media.Images.Image(),
                },
                new Art()
                {
                    Id = 2,
                    Image = new DAL.Entities.Media.Images.Image(),
                },
            };
        }

        private List<ArtDTO> GetArtsDTOList()
        {
            return new List<ArtDTO>()
            {
                new ArtDTO
                {
                    Id = 1,
                    Image = new ImageDTO(),
                },
                new ArtDTO
                {
                    Id = 2,
                    Image = new ImageDTO(),
                },
            };
        }

        private void MockRepositoryAndMapper(List<Art> artList, List<ArtDTO> artListDTO, List<StreetcodeContent> streetcodeListUserCanAccess)
        {
            _mockRepo
                .Setup(r => r.ArtRepository
                    .GetAllAsync(
                        It.IsAny<Expression<Func<Art, bool>>>(),
                        It.IsAny<Func<IQueryable<Art>,
                        IIncludableQueryable<Art, object>>>()))
                .ReturnsAsync(artList);

            _mockRepo.Setup(repo => repo.StreetcodeRepository
                    .FindAll(
                        It.IsAny<Expression<Func<StreetcodeContent, bool>>>(),
                        It.IsAny<Func<IQueryable<StreetcodeContent>,
                            IIncludableQueryable<StreetcodeContent, object>>>()))
                .Returns(streetcodeListUserCanAccess.AsQueryable().BuildMockDbSet().Object);

            _mockMapper.Setup(x => x.Map<IEnumerable<ArtDTO>>(It.IsAny<IEnumerable<object>>()))
            .Returns(artListDTO);
        }
    }
}
