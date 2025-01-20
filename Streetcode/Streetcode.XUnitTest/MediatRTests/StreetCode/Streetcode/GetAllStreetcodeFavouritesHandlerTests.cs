using System.Linq.Expressions;
using AutoMapper;
using FluentResults;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.GetAllFavourites;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Entities.Streetcode.Types;
using Streetcode.DAL.Enums;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Streetcode
{
    public class GetAllStreetcodeFavouritesHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> repository;
        private readonly Mock<IMapper> mapper;
        private readonly Mock<ILoggerService> mockLogger;

        public GetAllStreetcodeFavouritesHandlerTests()
        {
            this.repository = new Mock<IRepositoryWrapper>();
            this.mapper = new Mock<IMapper>();
            this.mockLogger = new Mock<ILoggerService>();
        }

        [Fact]
        public async Task Handle_ReturnsSuccessWithEmptyArray()
        {
            // Arrange
            this.SetupRepository(new List<StreetcodeContent>());

            var handler = new GetAllStreetcodeFavouritesHandler(this.mapper.Object, this.repository.Object, this.mockLogger.Object);
            string userId = "mockId";

            // Act
            var result = await handler.Handle(new GetAllStreetcodeFavouritesQuery(userId), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.IsType<Result<IEnumerable<StreetcodeFavouriteDTO>>>(result),
                () => Assert.IsAssignableFrom<IEnumerable<StreetcodeFavouriteDTO>>(result.Value),
                () => Assert.Empty(result.Value));
        }

        [Fact]
        public async Task Handle_ReturnsSuccessWithNotFilteredStreetcodes()
        {
            // Arrange
            this.SetupRepository(new List<StreetcodeContent>
            {
                new StreetcodeContent()
            });
            this.SetupMapper(new List<StreetcodeFavouriteDTO>
            {
                new StreetcodeFavouriteDTO()
            });

            var handler = new GetAllStreetcodeFavouritesHandler(this.mapper.Object, this.repository.Object, this.mockLogger.Object);
            string userId = "mockId";

            // Act
            var result = await handler.Handle(new GetAllStreetcodeFavouritesQuery(userId), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.IsType<Result<IEnumerable<StreetcodeFavouriteDTO>>>(result),
                () => Assert.IsAssignableFrom<IEnumerable<StreetcodeFavouriteDTO>>(result.Value),
                () => Assert.NotEmpty(result.Value));
        }

        [Fact]
        public async Task Handle_ReturnsSuccessWithFilteredStreetcodes()
        {
            // Arrange
            this.SetupRepository(new List<StreetcodeContent>
            {
                new PersonStreetcode()
            });
            this.SetupMapper(new List<StreetcodeFavouriteDTO>
            {
                new StreetcodeFavouriteDTO()
            });

            var handler = new GetAllStreetcodeFavouritesHandler(this.mapper.Object, this.repository.Object, this.mockLogger.Object);
            string userId = "mockId";
            StreetcodeType type = StreetcodeType.Person;

            // Act
            var result = await handler.Handle(new GetAllStreetcodeFavouritesQuery(userId, type), CancellationToken.None);

            // Assert
            foreach (var streetcode in result.Value)
            {
                Assert.Equal(streetcode.Type, StreetcodeType.Person);
            }
        }
        private void SetupRepository(List<StreetcodeContent> returnList)
        {
            this.repository.Setup(repo => repo.StreetcodeRepository.GetAllAsync(
                It.IsAny<Expression<Func<StreetcodeContent, bool>>>(),
                It.IsAny<Func<IQueryable<StreetcodeContent>,
                IIncludableQueryable<StreetcodeContent, object>>>()))
                .ReturnsAsync(returnList);
        }

        private void SetupMapper(List<StreetcodeFavouriteDTO> returnList)
        {
            this.mapper.Setup(x => x.Map<IEnumerable<StreetcodeFavouriteDTO>>(It.IsAny<IEnumerable<object>>()))
                .Returns(returnList);
        }
    }
}
