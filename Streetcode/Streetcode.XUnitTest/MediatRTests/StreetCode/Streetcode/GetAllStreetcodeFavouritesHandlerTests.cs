using System.Linq.Expressions;
using AutoMapper;
using FluentResults;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.GetAllFavourites;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Entities.Streetcode.Types;
using Streetcode.DAL.Enums;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Streetcode
{
    public class GetAllStreetcodeFavouritesHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> _repository;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessor;
        private readonly MockNoSharedResourceLocalizer _stringLocalizerNo;


        public GetAllStreetcodeFavouritesHandlerTests()
        {
            _repository = new Mock<IRepositoryWrapper>();
            _mapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILoggerService>();
            _httpContextAccessor = new Mock<IHttpContextAccessor>();
            _stringLocalizerNo = new MockNoSharedResourceLocalizer();
        }

        [Fact]
        public async Task Handle_ReturnsBadRequest()
        {
            // Arrange
            string userId = "mockId";

            this.SetupRepository(new List<StreetcodeContent>());
            string expectedError = _stringLocalizerNo["NoFavouritesFound"].Value;

            MockHelpers.SetupMockHttpContextAccessor(_httpContextAccessor, userId);

            var handler = new GetAllStreetcodeFavouritesHandler(_mapper.Object, _repository.Object, _mockLogger.Object, _stringLocalizerNo, _httpContextAccessor.Object);

            // Act
            var result = await handler.Handle(new GetAllStreetcodeFavouritesQuery(), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.Equal(expectedError, result.Errors[0].Message),
                () => Assert.True(result.IsFailed));
        }

        [Fact]
        public async Task Handle_ReturnsSuccessWithNotFilteredStreetcodes()
        {
            // Arrange
            string userId = "mockId";

            this.SetupRepository(new List<StreetcodeContent>
            {
                new StreetcodeContent()
            });
            this.SetupMapper(new List<StreetcodeFavouriteDto>
            {
                new StreetcodeFavouriteDto()
            });

            MockHelpers.SetupMockHttpContextAccessor(_httpContextAccessor, userId);

            var handler = new GetAllStreetcodeFavouritesHandler(_mapper.Object, _repository.Object, _mockLogger.Object, _stringLocalizerNo, _httpContextAccessor.Object);

            // Act
            var result = await handler.Handle(new GetAllStreetcodeFavouritesQuery(), CancellationToken.None);

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
            string userId = "mockId";

            this.SetupRepository(new List<StreetcodeContent>
            {
                new PersonStreetcode()
            });
            this.SetupMapper(new List<StreetcodeFavouriteDto>
            {
                new StreetcodeFavouriteDto()
            });


            MockHelpers.SetupMockHttpContextAccessor(_httpContextAccessor, userId);

            var handler = new GetAllStreetcodeFavouritesHandler(_mapper.Object, _repository.Object, _mockLogger.Object, _stringLocalizerNo, _httpContextAccessor.Object);

            StreetcodeType type = StreetcodeType.Person;

            // Act
            var result = await handler.Handle(new GetAllStreetcodeFavouritesQuery(type), CancellationToken.None);

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
