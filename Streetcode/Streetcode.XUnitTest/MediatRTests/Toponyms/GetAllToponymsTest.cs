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
            SetupPaginatedRepository(GetToponymList());
            SetupMapper(GetListToponymDto());

            var handler = new GetAllToponymsHandler(_mockRepository.Object, _mockMapper.Object);

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
            SetupPaginatedRepository(GetToponymList());
            SetupMapper(GetListToponymDto());

            var handler = new GetAllToponymsHandler(_mockRepository.Object, _mockMapper.Object);

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
            SetupPaginatedRepository(GetToponymList().Take(pageSize));
            SetupMapper(GetListToponymDto().Take(pageSize));

            var handler = new GetAllToponymsHandler(_mockRepository.Object, _mockMapper.Object);

            // Act
            var result = await handler.Handle(new GetAllToponymsQuery(new GetAllToponymsRequestDTO { Amount = pageSize }), CancellationToken.None);

            // Assert
            Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.IsType<GetAllToponymsResponseDTO>(result.Value),
            () => Assert.Equal(pageSize, result.Value.Toponyms.Count()));
        }

        [Fact]
        public async Task ReturnsEmptyResponse_EmptyToponymList()
        {
            // Arrange
            SetupPaginatedRepository(new List<Toponym>());
            SetupMapper(new List<ToponymDTO>());
            var handler = new GetAllToponymsHandler(_mockRepository.Object, _mockMapper.Object);

            // Act
            var result = await handler.Handle(new GetAllToponymsQuery(new GetAllToponymsRequestDTO()), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.NotNull(result),
                () => Assert.Empty(result.Value.Toponyms));
        }

        private static IEnumerable<Toponym> GetToponymList()
        {
            var toponyms = new List<Toponym>
            {
                new () { Id = 1 },
                new () { Id = 2 },
                new () { Id = 3 },
            };

            return toponyms;
        }

        private static List<ToponymDTO> GetListToponymDto()
        {
            var toponymsDto = new List<ToponymDTO>
            {
                new () { Id = 1 },
                new () { Id = 2 },
                new () { Id = 3 },
            };

            return toponymsDto;
        }

        private void SetupPaginatedRepository(IEnumerable<Toponym> returnList)
        {
            _mockRepository
                .Setup(repo => repo.ToponymRepository.GetAllPaginated(
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
            _mockMapper
                .Setup(x => x.Map<IEnumerable<ToponymDTO>>(
                    It.IsAny<IEnumerable<Toponym>>()))
                .Returns(returnList);
        }
    }
}
