using AutoMapper;
using FluentResults;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.MediatR.Media.StreetcodeArt.GetByStreetcodeId;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System.Linq.Expressions;
using Xunit;

namespace Streetcode.XUnitTest.MediaRTests.MediaTests.StreetcodeArtTest
{
    public class GetStreetcodeArtByStreetcodeIdTest
    {
        private Mock<IRepositoryWrapper> repository;
        private Mock<IMapper> mockMapper;
        
        public GetStreetcodeArtByStreetcodeIdTest()
        {
            repository = new Mock<IRepositoryWrapper>();
            mockMapper = new Mock<IMapper>();
        }

        [Theory]
        [InlineData(1)]

        public async Task GetStreetcodeArtByStreetcodeId_ReturnsCorrectStreetcodeArtByStreetcodeId(int streetcodeId)
        {
            var allStreetcodeArtsList = new List<StreetcodeArt>()
            {
                new StreetcodeArt()
                {
                    Index = 1,
                    StreetcodeId = 1,
                    Streetcode = null,
                    ArtId = 1,
                    Art=null

                },

                new StreetcodeArt()
                {
                    Index = 2,
                    StreetcodeId = 2,
                    Streetcode = null,
                    ArtId = 2,
                    Art=null
                }
            };

            var allStreetcodeArtsDTOList = new List<StreetcodeArtDTO>()
            {
                new StreetcodeArtDTO
                {
                    Index = 1,
                    StreetcodeId = 1,
                    Streetcode = null,
                    ArtId = 1,
                    Art=null
                },

                new StreetcodeArtDTO
                {
                    Index = 2,
                    StreetcodeId = 2,
                    Streetcode = null,
                    ArtId = 2,
                    Art=null
                }
            };

            repository.Setup(r => r.StreetcodeArtRepository.GetAllAsync(It.IsAny<Expression<Func<StreetcodeArt, bool>>>(),
                It.IsAny<Func<IQueryable<StreetcodeArt>, IIncludableQueryable<StreetcodeArt, object>>>()))
                .ReturnsAsync(allStreetcodeArtsList);

            mockMapper.Setup(x => x.Map<IEnumerable<StreetcodeArtDTO>>(It.IsAny<IEnumerable<object>>())).Returns(allStreetcodeArtsDTOList);

            var handler = new GetStreetcodeArtByStreetcodeIdHandler(repository.Object, mockMapper.Object);

            var result = await handler.Handle(new GetStreetcodeArtByStreetcodeIdQuery(streetcodeId), CancellationToken.None);

            Assert.Equal(streetcodeId, result.Value.First().ArtId);

        }

        [Theory]
        [InlineData(1)]

        public async Task GetStreetcodeArtByStreetcodeId_ReturnCorrectTypeResult(int streetcodeId)
        {
            var allStreetcodeArtsList = new List<StreetcodeArt>()
            {
                new StreetcodeArt()
                {
                    Index = 1,
                    StreetcodeId = 1,
                    Streetcode = null,
                    ArtId = 1,
                    Art=null

                },

                new StreetcodeArt()
                {
                    Index = 2,
                    StreetcodeId = 2,
                    Streetcode = null,
                    ArtId = 2,
                    Art=null
                }
            };

            var allStreetcodeArtsDTOList = new List<StreetcodeArtDTO>()
            {
                new StreetcodeArtDTO
                {
                    Index = 1,
                    StreetcodeId = 1,
                    Streetcode = null,
                    ArtId = 1,
                    Art=null
                },

                new StreetcodeArtDTO
                {
                    Index = 2,
                    StreetcodeId = 2,
                    Streetcode = null,
                    ArtId = 2,
                    Art=null
                }
            };

            repository.Setup(r => r.StreetcodeArtRepository.GetAllAsync(It.IsAny<Expression<Func<StreetcodeArt, bool>>>(),
               It.IsAny<Func<IQueryable<StreetcodeArt>, IIncludableQueryable<StreetcodeArt, object>>>()))
               .ReturnsAsync(allStreetcodeArtsList);

            mockMapper.Setup(x => x.Map<IEnumerable<StreetcodeArtDTO>>(It.IsAny<IEnumerable<object>>())).Returns(allStreetcodeArtsDTOList);

            var handler = new GetStreetcodeArtByStreetcodeIdHandler(repository.Object, mockMapper.Object);

            var result = await handler.Handle(new GetStreetcodeArtByStreetcodeIdQuery(streetcodeId), CancellationToken.None);

            Assert.IsType<Result<IEnumerable<StreetcodeArtDTO>>>(result);

        }

    }

}





