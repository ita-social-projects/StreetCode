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
using System;
using System.Linq.Expressions;
using Xunit;

namespace Streetcode.XUnitTest.MediaRTests.MediaTests.StreetcodeArtTest
{
  public class GetStreetcodeArtByStreetcodeIdTest
    {
        private readonly Mock<IRepositoryWrapper> repository;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<IBlobService> blobService;
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> _mockLocalizer;

        public GetStreetcodeArtByStreetcodeIdTest()
        {
            repository = new Mock<IRepositoryWrapper>();
            mockMapper = new Mock<IMapper>();
            blobService = new Mock<IBlobService>();
            _mockLogger = new Mock<ILoggerService>();
            _mockLocalizer = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        }

        private List<StreetcodeArt> GetStreetcodeArtsList()
        {
            return new List<StreetcodeArt>
            {
                new StreetcodeArt()
                {
                    Index = 1,
                    StreetcodeId = 1,
                    Streetcode = null,
                    ArtId = 1,
                    Art= new DAL.Entities.Media.Images.Art
                    {
                        Image = new DAL.Entities.Media.Images.Image()
                    }
                },

                new StreetcodeArt()
                {
                    Index = 2,
                    StreetcodeId = 2,
                    Streetcode = null,
                    ArtId = 2,
                    Art = new DAL.Entities.Media.Images.Art
                    {
                        Image = new DAL.Entities.Media.Images.Image()
                    }
                }
            };
        }

        private List<StreetcodeArtDTO> GetStreetcodeArtDTOList()
        {
            return new List<StreetcodeArtDTO>()
            {
                new StreetcodeArtDTO
                {
                    Index = 1,
                    StreetcodeId = 1,
                    Art = new ArtDTO
                    {
                        Image = new ImageDTO()
                    }
                },

                new StreetcodeArtDTO
                {
                    Index = 2,
                    StreetcodeId = 2,
                    Art = new ArtDTO
                    {
                        Image = new ImageDTO()
                    }
                }
            };
        }

        private void SetupRepository()
        {
            this.repository.Setup(r => r.StreetcodeArtRepository.GetAllAsync(It.IsAny<Expression<Func<StreetcodeArt, bool>>>(),
                It.IsAny<Func<IQueryable<StreetcodeArt>, IIncludableQueryable<StreetcodeArt, object>>>()))
                .ReturnsAsync(GetStreetcodeArtsList());
            this.repository.Setup(r => r.StreetcodeRepository
                .GetFirstOrDefaultAsync(It.IsAny<Expression<Func<StreetcodeContent, StreetcodeContent>>>(), null, null))
                .ReturnsAsync(It.IsAny<StreetcodeContent>());
        }

        [Theory]
        [InlineData(1)]
        public async Task GetStreetcodeArtByStreetcodeId_ReturnsSuccesfullyStreetcodeArt(int streetcodeId)
        {
            this.SetupRepository();

            this.mockMapper.Setup(x => x.Map<IEnumerable<StreetcodeArtDTO>>(It.IsAny<IEnumerable<object>>())).Returns(GetStreetcodeArtDTOList());

            var handler = new GetStreetcodeArtByStreetcodeIdHandler(repository.Object, mockMapper.Object, blobService.Object, _mockLogger.Object, _mockLocalizer.Object);

            var result = await handler.Handle(new GetStreetcodeArtByStreetcodeIdQuery(streetcodeId), CancellationToken.None);

            Assert.Equal(streetcodeId, result.Value.First().StreetcodeId);
        }

        [Theory]
        [InlineData(1)]
        public async Task GetStreetcodeArtByStreetcodeId_ReturnCorrectTypeResult(int streetcodeId)
        {
            SetupRepository();

            mockMapper.Setup(x => x.Map<IEnumerable<StreetcodeArtDTO>>(It.IsAny<IEnumerable<object>>())).Returns(GetStreetcodeArtDTOList());

            var handler = new GetStreetcodeArtByStreetcodeIdHandler(repository.Object, mockMapper.Object, blobService.Object, _mockLogger.Object, _mockLocalizer.Object);

            var result = await handler.Handle(new GetStreetcodeArtByStreetcodeIdQuery(streetcodeId), CancellationToken.None);

            Assert.IsType<Result<IEnumerable<StreetcodeArtDTO>>>(result);

        }

    }

}


