using System.Linq.Expressions;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.GetFavouriteById;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.DAL.Repositories.Interfaces.Streetcode;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Streetcode;

public class GetFavouriteByIdHandlerTests
{
    private readonly Mock<IRepositoryWrapper> _repository;
    private readonly Mock<IMapper> _mapper;
    private readonly Mock<ILoggerService> _mockLogger;
    private readonly MockNoSharedResourceLocalizer _stringLocalizerNo;
    private readonly Mock<IHttpContextAccessor> _httpContextAccessor;

    public GetFavouriteByIdHandlerTests()
    {
        _repository = new Mock<IRepositoryWrapper>();
        _mapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILoggerService>();
        _stringLocalizerNo = new MockNoSharedResourceLocalizer();
        _httpContextAccessor = new Mock<IHttpContextAccessor>();
    }

    [Fact]
    public async Task Handle_ReturnsBadRequest()
    {
        // Arrange
        RepositorySetup(null);

        const int incorrectId = -1;
        string expectedError = _stringLocalizerNo["NoFavouritesWithId", incorrectId];

        const string userId = "mockId";
        MockHelpers.SetupMockHttpContextAccessor(_httpContextAccessor, userId);
        var handler = new GetFavouriteByIdHandler(_mapper.Object, _repository.Object, _mockLogger.Object, _stringLocalizerNo, _httpContextAccessor.Object);

        // Act
        var result = await handler.Handle(new GetFavouriteByIdQuery(incorrectId), CancellationToken.None);

        // Asset
        Assert.Equal(expectedError, result.Errors.Single().Message);
    }

    [Fact]
    public async Task Handle_ReturnsSuccessfulResult()
    {
        const int streetcodeId = 1;
        const string userId = "mockId";

        RepositorySetup(
            new StreetcodeContent
            {
                Id = streetcodeId,
            });

        SetupMapper(
            new StreetcodeFavouriteDto
            {
                Id = streetcodeId,
            });

        MockHelpers.SetupMockHttpContextAccessor(_httpContextAccessor, userId);

        var handler = new GetFavouriteByIdHandler(_mapper.Object, _repository.Object, _mockLogger.Object, _stringLocalizerNo, _httpContextAccessor.Object);

        // Act
        var result = await handler.Handle(new GetFavouriteByIdQuery(streetcodeId), CancellationToken.None);

        // Asset
        Assert.Multiple(
            () => Assert.True(result.IsSuccess),
            () => Assert.Equal(streetcodeId, result.Value.Id));
    }

    private void RepositorySetup(StreetcodeContent? favourite)
    {
        var streetcodeRepoMock = new Mock<IStreetcodeRepository>();

        streetcodeRepoMock.Setup(x => x.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<StreetcodeContent, bool>>?>(), It.IsAny<Func<IQueryable<StreetcodeContent>, IIncludableQueryable<StreetcodeContent, object>>>()))
            .ReturnsAsync(favourite);

        _repository.Setup(x => x.StreetcodeRepository).Returns(streetcodeRepoMock.Object);
    }

    private void SetupMapper(StreetcodeFavouriteDto favourite)
    {
        _mapper.Setup(x => x.Map<StreetcodeFavouriteDto>(It.IsAny<object>()))
            .Returns(favourite);
    }
}