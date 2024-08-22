using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.Fact.Delete;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Facts;

public class DeleteFactTest
{
    private readonly Mock<IRepositoryWrapper> repository;
    private readonly Mock<ILoggerService> mockLogger;
    private readonly Mock<IStringLocalizer<FailedToDeleteSharedResource>> mockLocalizerFailedToDelete;
    private readonly Mock<IStringLocalizer<CannotFindSharedResource>> mockLocalizerCannotFind;

    public DeleteFactTest()
    {
        this.repository = new Mock<IRepositoryWrapper>();
        this.mockLogger = new Mock<ILoggerService>();
        this.mockLocalizerCannotFind = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        this.mockLocalizerFailedToDelete = new Mock<IStringLocalizer<FailedToDeleteSharedResource>>();
    }

    [Theory]
    [InlineData(2)]
    public async Task ShouldDeleteSuccessfully(int id)
    {
        // Arrange
        this.repository
            .Setup(x => x.FactRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Fact, bool>>>(),
                It.IsAny<Func<IQueryable<Fact>,
                IIncludableQueryable<Fact, object>>>()))
            .ReturnsAsync(GetFact(id));

        this.repository.Setup(x => x.FactRepository
            .Delete(GetFact(id)));

        this.repository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

        var handler = new DeleteFactHandler(this.repository.Object, this.mockLogger.Object, this.mockLocalizerFailedToDelete.Object, this.mockLocalizerCannotFind.Object);

        // Act
        var result = await handler.Handle(new DeleteFactCommand(id), CancellationToken.None);

        // Assert
        Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.True(result.IsSuccess));
    }

    [Theory]
    [InlineData(2)]
    public async Task ShouldThrowExeption_IdNotExisting(int id)
    {
        // Arrange
        this.repository
            .Setup(x => x.FactRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Fact, bool>>>(),
                It.IsAny<Func<IQueryable<Fact>,
                IIncludableQueryable<Fact, object>>>()))
            .ReturnsAsync(GetFactWithNotExistingId());

        this.repository
            .Setup(x => x.FactRepository
            .Delete(GetFactWithNotExistingId() !));

        var expectedError = $"Cannot find a fact with corresponding categoryId: {id}";
        this.mockLocalizerCannotFind.Setup(x => x[It.IsAny<string>(), It.IsAny<object>()]).Returns((string key, object[] args) =>
        {
            if (args != null && args.Length > 0 && args[0] is int id)
            {
                return new LocalizedString(key, $"Cannot find a fact with corresponding categoryId: {id}");
            }

            return new LocalizedString(key, "Cannot find any fact with unknown categoryId");
        });

        // Act
        var handler = new DeleteFactHandler(this.repository.Object, this.mockLogger.Object, this.mockLocalizerFailedToDelete.Object, this.mockLocalizerCannotFind.Object);

        var result = await handler.Handle(new DeleteFactCommand(id), CancellationToken.None);

        // Assert
        Assert.Equal(expectedError, result.Errors[0].Message);
    }

    [Theory]
    [InlineData(2)]
    public async Task ShouldThrowExeption_SaveChangesAsyncIsNotSuccessful(int id)
    {
        // Arrange
        this.repository
            .Setup(x => x.FactRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Fact, bool>>>(),
                It.IsAny<Func<IQueryable<Fact>,
                IIncludableQueryable<Fact, object>>>()))
            .ReturnsAsync(GetFact(id));

        this.repository.Setup(x => x.FactRepository
            .Delete(GetFact(id)));

        this.repository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(0);

        var expectedError = "Failed to delete a fact";
        this.mockLocalizerFailedToDelete.Setup(x => x["FailedToDeleteFact"])
           .Returns(new LocalizedString("FailedToDeleteFact", expectedError));

        // Act
        var handler = new DeleteFactHandler(this.repository.Object, this.mockLogger.Object, this.mockLocalizerFailedToDelete.Object, this.mockLocalizerCannotFind.Object);

        var result = await handler.Handle(new DeleteFactCommand(id), CancellationToken.None);

        // Assert
        Assert.Equal(expectedError, result.Errors[0].Message);
    }

    private static Fact GetFact(int id)
    {
        return new Fact
        {
            Id = id,
        };
    }

    private static Fact? GetFactWithNotExistingId()
    {
        return null;
    }
}
