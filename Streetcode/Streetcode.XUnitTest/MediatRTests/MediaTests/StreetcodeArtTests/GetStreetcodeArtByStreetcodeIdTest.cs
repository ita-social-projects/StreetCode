using System.Linq.Expressions;
using AutoMapper;
using FluentResults;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Media.Art;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Media.StreetcodeArt.GetByStreetcodeId;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Enums;
using Streetcode.DAL.Repositories.Interfaces.Base;

using Xunit;

namespace Streetcode.XUnitTest.MediaRTests.MediaTests.StreetcodeArtTest
{
    public class GetStreetcodeArtByStreetcodeIdTest
    {
        private readonly Mock<IRepositoryWrapper> _repository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IBlobService> _blobService;
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> _mockLocalizer;

        public GetStreetcodeArtByStreetcodeIdTest()
        {
            _repository = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _blobService = new Mock<IBlobService>();
            _mockLogger = new Mock<ILoggerService>();
            _mockLocalizer = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        }

        [Theory]
        [InlineData(1)]
        public async Task GetStreetcodeArtByStreetcodeId_ReturnsSuccesfullyStreetcodeArt(int streetcodeId)
        {
            SetupRepository();

            _mockMapper.Setup(x => x.Map<IEnumerable<StreetcodeArtDTO>>(It.IsAny<IEnumerable<object>>())).Returns(GetStreetcodeArtDtoList());

            var handler = new GetStreetcodeArtByStreetcodeIdHandler(_repository.Object, _mockMapper.Object, _blobService.Object, _mockLogger.Object, _mockLocalizer.Object);

            var result = await handler.Handle(new GetStreetcodeArtByStreetcodeIdQuery(streetcodeId, UserRole.User), CancellationToken.None);

            Assert.True(result.Value.All(a => GetStreetcodeArtDtoList()[a.Index - 1].Art!.Id == a.Art?.Id));
        }

        [Theory]
        [InlineData(1)]
        public async Task GetStreetcodeArtByStreetcodeId_ReturnCorrectTypeResult(int streetcodeId)
        {
            SetupRepository();

            _mockMapper.Setup(x => x.Map<IEnumerable<StreetcodeArtDTO>>(It.IsAny<IEnumerable<object>>())).Returns(GetStreetcodeArtDtoList());

            var handler = new GetStreetcodeArtByStreetcodeIdHandler(_repository.Object, _mockMapper.Object, _blobService.Object, _mockLogger.Object, _mockLocalizer.Object);

            var result = await handler.Handle(new GetStreetcodeArtByStreetcodeIdQuery(streetcodeId, UserRole.User), CancellationToken.None);

            Assert.IsType<Result<IEnumerable<StreetcodeArtDTO>>>(result);
        }

        private static List<StreetcodeArt> GetStreetcodeArtsList()
        {
            return new List<StreetcodeArt>
            {
                new StreetcodeArt()
                {
                    Index = 1,
                    StreetcodeArtSlideId = 1,
                    StreetcodeArtSlide = null,
                    ArtId = 1,
                    Art = new DAL.Entities.Media.Images.Art
                    {
                        Image = new DAL.Entities.Media.Images.Image(),
                    },
                },

                new StreetcodeArt()
                {
                    Index = 2,
                    StreetcodeArtSlideId = 2,
                    StreetcodeArtSlide = null,
                    ArtId = 2,
                    Art = new DAL.Entities.Media.Images.Art
                    {
                        Image = new DAL.Entities.Media.Images.Image(),
                    },
                },
            };
        }

        private static List<StreetcodeArtDTO> GetStreetcodeArtDtoList()
        {
            return new List<StreetcodeArtDTO>()
            {
                new StreetcodeArtDTO
                {
                    Index = 1,
                    Art = new ArtDTO
                    {
                        Image = new ImageDTO(),
                    },
                },

                new StreetcodeArtDTO
                {
                    Index = 2,
                    Art = new ArtDTO
                    {
                        Image = new ImageDTO(),
                    },
                },
            };
        }

        private void SetupRepository()
        {
            _repository.Setup(r => r.StreetcodeArtRepository.GetAllAsync(
                It.IsAny<Expression<Func<StreetcodeArt, bool>>>(),
                It.IsAny<Func<IQueryable<StreetcodeArt>, IIncludableQueryable<StreetcodeArt, object>>>()))
                .ReturnsAsync(GetStreetcodeArtsList());
            _repository.Setup(r => r.StreetcodeRepository
                .GetFirstOrDefaultAsync(It.IsAny<Expression<Func<StreetcodeContent, StreetcodeContent>>>(), null, null))
                .ReturnsAsync(It.IsAny<StreetcodeContent>());
        }
    }
}
