using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Toponyms;
using Streetcode.BLL.MediatR.Toponyms.GetAll;
using Streetcode.DAL.Entities.Toponyms;
using Streetcode.DAL.Helpers;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Toponyms
{
    public class GetAllToponymsTest
    {
        private readonly Mock<IRepositoryWrapper> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;

        public GetAllToponymsTest()
        {
            _mockRepository = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
        }

        [Fact]
        public async Task ReturnsSuccessfully_CorrectType()
        {
            // Arrange
            this.SetupPaginatedRepository(GetToponymList());
            this.SetupMapper(GetListToponymDTO());

            var handler = new GetAllToponymsHandler(this._mockRepository.Object, this._mockMapper.Object);

            // Act
            var result = await handler.Handle(new GetAllToponymsQuery(new GetAllToponymsRequestDTO()), CancellationToken.None);

            // Assert
            Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.IsType<GetAllToponymsResponseDTO>(result.Value));
        }

        [Fact]
        public async Task ReturnsSuccessfully_CountMatch()
        {
            // Arrange
            this.SetupPaginatedRepository(GetToponymList());
            this.SetupMapper(GetListToponymDTO());

            var handler = new GetAllToponymsHandler(this._mockRepository.Object, this._mockMapper.Object);

            // Act
            var result = await handler.Handle(new GetAllToponymsQuery(new GetAllToponymsRequestDTO()), CancellationToken.None);

            // Assert
            Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.Equal(GetToponymList().Count(), result.Value.Toponyms.Count()));
        }

        [Fact]
        public async Task Handler_Returns_Correct_PageSize()
        {
            // Arrange
            ushort pageSize = 1;
            this.SetupPaginatedRepository(GetToponymList().Take(pageSize));
            this.SetupMapper(GetListToponymDTO().Take(pageSize));

            var handler = new GetAllToponymsHandler(this._mockRepository.Object, this._mockMapper.Object);

            // Act
            var result = await handler.Handle(new GetAllToponymsQuery(new GetAllToponymsRequestDTO { Amount = pageSize }), CancellationToken.None);

            // Assert
            Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.IsType<GetAllToponymsResponseDTO>(result.Value),
            () => Assert.Equal(pageSize, result.Value.Toponyms.Count()));
        }

        private static IEnumerable<Toponym> GetToponymList()
        {
            var toponyms = new List<Toponym>
            {
                new ()
                {
                    Id = 1,
                },
                new ()
                {
                    Id = 2,
                },
                new ()
                {
                    Id = 3,
                },
            };

            return toponyms;
        }

        private static List<ToponymDTO> GetListToponymDTO()
        {
            var toponymsDTO = new List<ToponymDTO>
            {
                new ()
                {
                    Id = 1,
                },
                new ()
                {
                    Id = 2,
                },
                new ()
                {
                    Id = 3,
                },
            };

            return toponymsDTO;
        }

        private void SetupPaginatedRepository(IEnumerable<Toponym> returnList)
        {
            this._mockRepository.Setup(repo => repo.ToponymRepository.GetAllPaginated(
            It.IsAny<ushort?>(),
            It.IsAny<ushort?>(),
            It.IsAny<Expression<Func<Toponym, Toponym>>?>(),
            It.IsAny<Expression<Func<Toponym, bool>>?>(),
            It.IsAny<Func<IQueryable<Toponym>, IIncludableQueryable<Toponym, object>>?>(),
            It.IsAny<Expression<Func<Toponym, object>>?>(),
            It.IsAny<Expression<Func<Toponym, object>>?>()))
        .Returns(PaginationResponse<Toponym>.Create(returnList.AsQueryable()));
        }

        private void SetupMapper(IEnumerable<ToponymDTO> returnList)
        {
            this._mockMapper
                .Setup(x => x.Map<IEnumerable<ToponymDTO>>(It.IsAny<IEnumerable<Toponym>>()))
                .Returns(returnList);
        }
    }
}
