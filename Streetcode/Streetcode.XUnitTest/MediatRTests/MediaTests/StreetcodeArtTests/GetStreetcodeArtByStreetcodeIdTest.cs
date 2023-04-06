﻿using AutoMapper;
using FluentResults;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.MediatR.Media.StreetcodeArt.GetByStreetcodeId;
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
        private Mock<IBlobService> blobService;

        public GetStreetcodeArtByStreetcodeIdTest()
        {
            repository = new Mock<IRepositoryWrapper>();
            mockMapper = new Mock<IMapper>();
            blobService = new Mock<IBlobService>();
        }

        private List<StreetcodeArt> GetStreetcodeArtsList()
        {
            return new List<StreetcodeArt>
            { new StreetcodeArt()
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
                    Streetcode = null,
                    ArtId = 1,
                    Art = new ArtDTO
                    {
                        Image = new ImageDTO()
                    }
                },

                new StreetcodeArtDTO
                {
                    Index = 2,
                    StreetcodeId = 2,
                    Streetcode = null,
                    ArtId = 2,
                    Art = new ArtDTO
                    {
                        Image = new ImageDTO()
                    }
                }
            };
        }

        [Theory]
        [InlineData(1)]
        public async Task GetStreetcodeArtByStreetcodeId_ReturnsSuccesfullyStreetcodeArt(int streetcodeId)
        {
            repository.Setup(r => r.StreetcodeArtRepository.GetAllAsync(It.IsAny<Expression<Func<StreetcodeArt, bool>>>(),
                It.IsAny<Func<IQueryable<StreetcodeArt>, IIncludableQueryable<StreetcodeArt, object>>>()))
                .ReturnsAsync(GetStreetcodeArtsList());

            mockMapper.Setup(x => x.Map<IEnumerable<StreetcodeArtDTO>>(It.IsAny<IEnumerable<object>>())).Returns(GetStreetcodeArtDTOList());

            var handler = new GetStreetcodeArtByStreetcodeIdHandler(repository.Object, mockMapper.Object, blobService.Object);

            var result = await handler.Handle(new GetStreetcodeArtByStreetcodeIdQuery(streetcodeId), CancellationToken.None);

            Assert.Equal(streetcodeId, result.Value.First().ArtId);
        }

        [Theory]
        [InlineData(1)]

        public async Task GetStreetcodeArtByStreetcodeId_ReturnCorrectTypeResult(int streetcodeId)
        {
               repository.Setup(r => r.StreetcodeArtRepository.GetAllAsync(It.IsAny<Expression<Func<StreetcodeArt, bool>>>(),
               It.IsAny<Func<IQueryable<StreetcodeArt>, IIncludableQueryable<StreetcodeArt, object>>>()))
               .ReturnsAsync(GetStreetcodeArtsList());

            mockMapper.Setup(x => x.Map<IEnumerable<StreetcodeArtDTO>>(It.IsAny<IEnumerable<object>>())).Returns(GetStreetcodeArtDTOList());

            var handler = new GetStreetcodeArtByStreetcodeIdHandler(repository.Object, mockMapper.Object, blobService.Object);

            var result = await handler.Handle(new GetStreetcodeArtByStreetcodeIdQuery(streetcodeId), CancellationToken.None);

            Assert.IsType<Result<IEnumerable<StreetcodeArtDTO>>>(result);

        }

    }

}


