using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Streetcode.TextContent.Fact;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.Fact.GetById;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Facts;

public class GetFactByIdTest
{
    private readonly Mock<IRepositoryWrapper> mockRepository;
    private readonly Mock<IMapper> mockMapper;
    private readonly Mock<ILoggerService> mockLogger;
    private readonly Mock<IStringLocalizer<CannotFindSharedResource>> mockLocalizerCannotFind;

    public GetFactByIdTest()
    {
        this.mockMapper = new Mock<IMapper>();
        this.mockRepository = new Mock<IRepositoryWrapper>();
        this.mockLogger = new Mock<ILoggerService>();
        this.mockLocalizerCannotFind = new Mock<IStringLocalizer<CannotFindSharedResource>>();
    }

    [Theory]
    [InlineData(1)]
    public async Task ShouldReturnSuccessfully_ExistingId(int id)
    {
        // Arrange
        this.mockRepository
            .Setup(x => x.FactRepository
                .GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<Fact, bool>>>(),
                    It.IsAny<Func<IQueryable<Fact>,
                    IIncludableQueryable<Fact, object>>>()))
            .ReturnsAsync(GetFact(id));

        this.mockMapper
            .Setup(x => x
            .Map<FactDto>(It.IsAny<Fact>()))
            .Returns(GetFactDTO(id));

        var handler = new GetFactByIdHandler(this.mockRepository.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizerCannotFind.Object);

        // Act
        var result = await handler.Handle(new GetFactByIdQuery(id), CancellationToken.None);

        // Assert
        Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.True(result.IsSuccess),
            () => Assert.Equal(result.Value.Id, id));
    }

    [Theory]
    [InlineData(1)]
    public async Task ShouldReturnSuccessfully_NotExistingId(int id)
    {
        // Arrange
        this.mockRepository
            .Setup(x => x.FactRepository
                .GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<Fact, bool>>>(),
                    It.IsAny<Func<IQueryable<Fact>,
                    IIncludableQueryable<Fact, object>>>()))
            .ReturnsAsync(GetFactWithNotExistingId());

        this.mockMapper
            .Setup(x => x.Map<FactDto?>(It.IsAny<Fact>()))
            .Returns(GetFactDTOWithNotExistingId());

        var expectedError = $"Cannot find any fact with corresponding id: {id}";
        this.mockLocalizerCannotFind.Setup(x => x[It.IsAny<string>(), It.IsAny<object>()]).Returns((string key, object[] args) =>
        {
            if (args != null && args.Length > 0 && args[0] is int id)
            {
                return new LocalizedString(key, $"Cannot find any fact with corresponding id: {id}");
            }

            return new LocalizedString(key, "Cannot find any fact with unknown categoryId");
        });

        var handler = new GetFactByIdHandler(this.mockRepository.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizerCannotFind.Object);

        // Act
        var result = await handler.Handle(new GetFactByIdQuery(id), CancellationToken.None);

        // Assert
        Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.True(result.IsFailed),
            () => Assert.Equal(expectedError, result.Errors[0].Message));
    }

    [Theory]
    [InlineData(1)]
    public async Task ShouldReturnSuccessfully_CorrectType(int id)
    {
        // Arrange
        this.mockRepository
            .Setup(x => x.FactRepository
                .GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<Fact, bool>>>(),
                    It.IsAny<Func<IQueryable<Fact>,
                    IIncludableQueryable<Fact, object>>>()))
            .ReturnsAsync(GetFact(id));

        this.mockMapper
            .Setup(x => x
            .Map<FactDto>(It.IsAny<Fact>()))
            .Returns(GetFactDTO(id));

        var handler = new GetFactByIdHandler(this.mockRepository.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizerCannotFind.Object);

        // Act
        var result = await handler.Handle(new GetFactByIdQuery(id), CancellationToken.None);

        // Assert
        Assert.Multiple(
            () => Assert.NotNull(result.ValueOrDefault),
            () => Assert.IsType<FactDto>(result.ValueOrDefault));
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

    private static FactDto GetFactDTO(int id)
    {
        return new FactDto
        {
            Id = id,
        };
    }

    private static FactDto? GetFactDTOWithNotExistingId()
    {
        return null;
    }
}
