using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.GetFavouriteStatus;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Streetcode;

public class GetFavouriteStatusHandlerTests
{
    private readonly Mock<IRepositoryWrapper> _repository;
    private readonly Mock<IHttpContextAccessor> _httpContextAccessor;

    public GetFavouriteStatusHandlerTests()
    {
        _repository = new Mock<IRepositoryWrapper>();
        _httpContextAccessor = new Mock<IHttpContextAccessor>();
    }

    [Fact]
    public async Task Handle_ReturnsTrue()
    {
        // Arrange
        const int streetcodeId = 1;
        const string userId = "mockId";

        RepositorySetup(
            new StreetcodeContent
            {
                Id = streetcodeId,
            });

        MockHelpers.SetupMockHttpContextAccessor(_httpContextAccessor, userId);
        var handler = new GetFavouriteStatusHandler(_repository.Object, _httpContextAccessor.Object);

        // Act
        var result = await handler.Handle(new GetFavouriteStatusQuery(streetcodeId), CancellationToken.None);

        // Assert
        Assert.True(result.Value);
    }

    [Fact]
    public async Task Handle_ReturnsFalse()
    {
        // Arrange
        const int streetcodeId = 1;
        const string userId = "mockId";

        RepositorySetup(null);
        MockHelpers.SetupMockHttpContextAccessor(_httpContextAccessor, userId);
        var handler = new GetFavouriteStatusHandler(_repository.Object, _httpContextAccessor.Object);

        // Act
        var result = await handler.Handle(new GetFavouriteStatusQuery(streetcodeId), CancellationToken.None);

        // Assert
        Assert.False(result.Value);
    }

    private void RepositorySetup(StreetcodeContent? favourite)
    {
        _repository.Setup(x => x.StreetcodeRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<StreetcodeContent, bool>>?>(), It.IsAny<Func<IQueryable<StreetcodeContent>, IIncludableQueryable<StreetcodeContent, object>>>()))
            .ReturnsAsync(favourite);
    }
}