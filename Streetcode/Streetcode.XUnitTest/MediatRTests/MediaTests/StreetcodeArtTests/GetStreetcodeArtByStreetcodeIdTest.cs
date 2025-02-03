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
using Streetcode.DAL.Repositories.Interfaces.Base;

using Xunit;

namespace Streetcode.XUnitTest.MediaRTests.MediaTests.StreetcodeArtTest
{
    public class GetStreetcodeArtByStreetcodeIdTest
    {
        private readonly Mock<IRepositoryWrapper> repository;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<IBlobService> blobService;
        private readonly Mock<ILoggerService> mockLogger;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> mockLocalizer;

        public GetStreetcodeArtByStreetcodeIdTest()
        {
            this.repository = new Mock<IRepositoryWrapper>();
            this.mockMapper = new Mock<IMapper>();
            this.blobService = new Mock<IBlobService>();
            this.mockLogger = new Mock<ILoggerService>();
            this.mockLocalizer = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        }

        [Theory]
        [InlineData(1)]
        public async Task GetStreetcodeArtByStreetcodeId_ReturnsSuccesfullyStreetcodeArt(int streetcodeId)
        {
            this.SetupRepository();

            this.mockMapper.Setup(x => x.Map<IEnumerable<StreetcodeArtDto>>(It.IsAny<IEnumerable<object>>())).Returns(this.GetStreetcodeArtDTOList());

            var handler = new GetStreetcodeArtByStreetcodeIdHandler(this.repository.Object, this.mockMapper.Object, this.blobService.Object, this.mockLogger.Object, this.mockLocalizer.Object);

            var result = await handler.Handle(new GetStreetcodeArtByStreetcodeIdQuery(streetcodeId), CancellationToken.None);

            Assert.True(result.Value.All(a => this.GetStreetcodeArtDTOList()[a.Index - 1].Art!.Id == a.Art?.Id));
        }

        [Theory]
        [InlineData(1)]
        public async Task GetStreetcodeArtByStreetcodeId_ReturnCorrectTypeResult(int streetcodeId)
        {
            this.SetupRepository();

            this.mockMapper.Setup(x => x.Map<IEnumerable<StreetcodeArtDto>>(It.IsAny<IEnumerable<object>>())).Returns(this.GetStreetcodeArtDTOList());

            var handler = new GetStreetcodeArtByStreetcodeIdHandler(this.repository.Object, this.mockMapper.Object, this.blobService.Object, this.mockLogger.Object, this.mockLocalizer.Object);

            var result = await handler.Handle(new GetStreetcodeArtByStreetcodeIdQuery(streetcodeId), CancellationToken.None);

            Assert.IsType<Result<IEnumerable<StreetcodeArtDto>>>(result);
        }

        private void SetupRepository()
        {
            this.repository.Setup(r => r.StreetcodeArtRepository.GetAllAsync(
                It.IsAny<Expression<Func<StreetcodeArt, bool>>>(),
                It.IsAny<Func<IQueryable<StreetcodeArt>, IIncludableQueryable<StreetcodeArt, object>>>()))
                .ReturnsAsync(this.GetStreetcodeArtsList());
            this.repository.Setup(r => r.StreetcodeRepository
                .GetFirstOrDefaultAsync(It.IsAny<Expression<Func<StreetcodeContent, StreetcodeContent>>>(), null, null))
                .ReturnsAsync(It.IsAny<StreetcodeContent>());
        }

        private List<StreetcodeArt> GetStreetcodeArtsList()
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

        private List<StreetcodeArtDto> GetStreetcodeArtDTOList()
        {
            return new List<StreetcodeArtDto>()
            {
                new StreetcodeArtDto
                {
                    Index = 1,
                    Art = new ArtDto
                    {
                        Image = new ImageDto(),
                    },
                },

                new StreetcodeArtDto
                {
                    Index = 2,
                    Art = new ArtDto
                    {
                        Image = new ImageDto(),
                    },
                },
            };
        }
    }
}
