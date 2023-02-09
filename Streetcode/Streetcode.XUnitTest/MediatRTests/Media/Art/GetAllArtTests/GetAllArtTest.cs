using Moq;
using Streetcode.BLL.MediatR.Media.Art.GetAll;
using Streetcode.BLL.DTO.Media.Images;
using AutoMapper;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using FluentResults;
using Streetcode.DAL.Entities.Media.Images;

namespace Streetcode.XUnitTest.MediatRTests.Media.Arts
{
    public class GetAllArtsTest
    {
        private readonly Mock<IRepositoryWrapper> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;

        public GetAllArtsTest()
        {
            _mockRepo = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
        }

        [Fact]
        public async Task Handle_ReturnsAllArts()
        {
            // Arrange
            MockRepositoryAndMapper(GetArtsList(), GetArtsDTOList());
            var handler = new GetAllArtsHandler(_mockRepo.Object, _mockMapper.Object);

            // Act
            var result = await handler.Handle(new GetAllArtsQuery(), default);

            // Assert
            Assert.Equal(GetArtsList().Count(), result.Value.Count());
        }


        [Fact]
        public async Task Handle_ReturnsZero()
        {
            //Arrange
            MockRepositoryAndMapper(new List<Art>(){ }, new List<ArtDTO>() { });
            var handler = new GetAllArtsHandler(_mockRepo.Object, _mockMapper.Object);
            int expectedResult = 0;
            
            //Act
            var result = await handler.Handle(new GetAllArtsQuery(), default);

            //Assert
            Assert.Equal(expectedResult, result.Value.Count());

        }

        [Fact]
        public async Task Handle_ReturnsType()
        {
            //Arrange
            MockRepositoryAndMapper(GetArtsList(), GetArtsDTOList());
            var handler = new GetAllArtsHandler(_mockRepo.Object, _mockMapper.Object);

            //Act
            var result = await handler.Handle(new GetAllArtsQuery(), default);

            //Assert
            Assert.IsType<Result<IEnumerable<ArtDTO>>>(result);
        }


        private List<Art> GetArtsList()
        {
             return  new List<Art>()
             {
                new Art()
                {
                    Id = 1,
                    ImageId = 1,
                    Description = "Test text 1",

                },
                new Art()
                {
                    Id = 2,
                    ImageId = 2,
                    Description = "Test text 2",

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
                    ImageId = 1,
                },
                new ArtDTO
                {
                    Id = 2,
                    ImageId = 2,
                },
            };
        }

        private void MockRepositoryAndMapper(List<Art> artList, List<ArtDTO> artListDTO)
        {
           _mockRepo.Setup(r => r.ArtRepository.GetAllAsync(
           It.IsAny<Expression<Func<Art, bool>>>(),
           It.IsAny<Func<IQueryable<Art>,
           IIncludableQueryable<Art, object>>>()))
           .ReturnsAsync(artList);

           _mockMapper.Setup(x => x.Map<IEnumerable<ArtDTO>>(It.IsAny<IEnumerable<object>>()))
           .Returns(artListDTO);
        }
    }
}
