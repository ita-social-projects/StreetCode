using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.GetFavouriteById;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Streetcode
{
    public class GetFavouriteByIdHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> _repository;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly Mock<IStringLocalizer<NoSharedResource>> _stringLocalizerNo;

        public GetFavouriteByIdHandlerTests()
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
            this.RepositorySetup(null);
            this.SetupLocalizers();

            int incorrectId = -1;
            string expectedError = $"No favourite streetcode with corresponding id: {incorrectId}";

            string userId = "mockId";

            var handler = new GetFavouriteByIdHandler(_mapper.Object, _repository.Object, _mockLogger.Object, _stringLocalizerNo.Object);

            // Act
            var result = await handler.Handle(new GetFavouriteByIdQuery(incorrectId, userId), CancellationToken.None);

            // Asset
            Assert.Equal(expectedError, result.Errors.Single().Message);
        }

        [Fact]
        public async Task Handle_ReturnsSuccessfulResult()
        {
            int streetcodeId = 1;
            string userId = "mockId";

            this.RepositorySetup(
                new StreetcodeContent
                {
                    Id = streetcodeId,
                });

            this.SetupMapper(
                new StreetcodeFavouriteDto
                {
                    Id = streetcodeId,
                });

            var handler = new GetFavouriteByIdHandler(_mapper.Object, _repository.Object, _mockLogger.Object, _stringLocalizerNo.Object);

            // Act
            var result = await handler.Handle(new GetFavouriteByIdQuery(streetcodeId, userId), CancellationToken.None);

            // Asset
            Assert.Multiple(
                () => Assert.True(result.IsSuccess),
                () => Assert.Equal(streetcodeId, result.Value.Id));
        }

        private void RepositorySetup(StreetcodeContent? favourite)
        {
            _repository.Setup(x => x.StreetcodeRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<StreetcodeContent, bool>>?>(), It.IsAny<Func<IQueryable<StreetcodeContent>, IIncludableQueryable<StreetcodeContent, object>>>()))
                .ReturnsAsync(favourite);

        }

        private void SetupMapper(StreetcodeFavouriteDto favourite)
        {
            _mapper.Setup(x => x.Map<StreetcodeFavouriteDto>(It.IsAny<object>()))
                .Returns(favourite);
        }

        private void SetupLocalizers()
        {
            _stringLocalizerNo.Setup(x => x[It.IsAny<string>(), It.IsAny<object>()])
               .Returns((string key, object[] args) =>
               {
                   if (args != null && args.Length > 0 && args[0] is int id)
                   {
                       return new LocalizedString(key, $"No favourite streetcode with corresponding id: {id}");
                   }

                   return new LocalizedString(key, "No favourite streetcode with unknown index");
               });
        }
    }
}
