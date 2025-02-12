using System.Linq.Expressions;
using AutoMapper;
using FluentResults;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.GetAllFavourites;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Entities.Streetcode.Types;
using Streetcode.DAL.Enums;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Streetcode
{
    public class GetAllStreetcodeFavouritesHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> _repository;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly Mock<IStringLocalizer<NoSharedResource>> _stringLocalizerNo;


        public GetAllStreetcodeFavouritesHandlerTests()
        {
            _repository = new Mock<IRepositoryWrapper>();
            _mapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILoggerService>();
            _stringLocalizerNo = new Mock<IStringLocalizer<NoSharedResource>>();
        }

        [Fact]
        public async Task Handle_ReturnsBadRequest()
        {
            // Arrange
            this.SetupRepository(new List<StreetcodeContent>());
            string expectedError = "No streetcode has been added to favourites";
            this._stringLocalizerNo.Setup(x => x["NoFavouritesFound"])
                .Returns(new LocalizedString("NoFavouritesFound", expectedError));

            var handler = new GetAllStreetcodeFavouritesHandler(_mapper.Object, _repository.Object, _mockLogger.Object, _stringLocalizerNo.Object);
            string userId = "mockId";

            // Act
            var result = await handler.Handle(new GetAllStreetcodeFavouritesQuery(userId), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.Equal(expectedError, result.Errors[0].Message),
                () => Assert.True(result.IsFailed));
        }

        [Fact]
        public async Task Handle_ReturnsSuccessWithNotFilteredStreetcodes()
        {
            // Arrange
            this.SetupRepository(new List<StreetcodeContent>
            {
                new StreetcodeContent()
            });
            this.SetupMapper(new List<StreetcodeFavouriteDto>
            {
                new StreetcodeFavouriteDto()
            });

            var handler = new GetAllStreetcodeFavouritesHandler(_mapper.Object, _repository.Object, _mockLogger.Object, _stringLocalizerNo.Object);
            string userId = "mockId";

            // Act
            var result = await handler.Handle(new GetAllStreetcodeFavouritesQuery(userId), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.IsType<Result<IEnumerable<StreetcodeFavouriteDto>>>(result),
                () => Assert.IsAssignableFrom<IEnumerable<StreetcodeFavouriteDto>>(result.Value),
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
            this.SetupMapper(new List<StreetcodeFavouriteDto>
            {
                new StreetcodeFavouriteDto()
            });

            var handler = new GetAllStreetcodeFavouritesHandler(_mapper.Object, _repository.Object, _mockLogger.Object, _stringLocalizerNo.Object);
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
            _repository.Setup(repo => repo.StreetcodeRepository.GetAllAsync(
                It.IsAny<Expression<Func<StreetcodeContent, bool>>>(),
                It.IsAny<Func<IQueryable<StreetcodeContent>,
                IIncludableQueryable<StreetcodeContent, object>>>()))
                .ReturnsAsync(returnList);
        }

        private void SetupMapper(List<StreetcodeFavouriteDto> returnList)
        {
            _mapper.Setup(x => x.Map<IEnumerable<StreetcodeFavouriteDto>>(It.IsAny<IEnumerable<object>>()))
                .Returns(returnList);
        }
    }
}
