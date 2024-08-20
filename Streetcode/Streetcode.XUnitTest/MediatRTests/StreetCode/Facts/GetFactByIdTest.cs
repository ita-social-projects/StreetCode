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
    private readonly Mock<IRepositoryWrapper> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILoggerService> _mockLogger;
    private readonly Mock<IStringLocalizer<CannotFindSharedResource>> _mockLocalizerCannotFind;

    public GetFactByIdTest()
    {
        this._mockMapper = new Mock<IMapper>();
        this._mockRepository = new Mock<IRepositoryWrapper>();
        this._mockLogger = new Mock<ILoggerService>();
        this._mockLocalizerCannotFind = new Mock<IStringLocalizer<CannotFindSharedResource>>();
    }

    [Theory]
    [InlineData(1)]
    public async Task ShouldReturnSuccessfully_ExistingId(int id)
    {
        // Arrange
        this._mockRepository
            .Setup(x => x.FactRepository
                .GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<Fact, bool>>>(),
                    It.IsAny<Func<IQueryable<Fact>,
                    IIncludableQueryable<Fact, object>>>()))
            .ReturnsAsync(GetFact(id));

        this._mockMapper
            .Setup(x => x
            .Map<FactDto>(It.IsAny<Fact>()))
            .Returns(GetFactDTO(id));

        var handler = new GetFactByIdHandler(this._mockRepository.Object, this._mockMapper.Object, this._mockLogger.Object, this._mockLocalizerCannotFind.Object);

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
        this._mockRepository
            .Setup(x => x.FactRepository
                .GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<Fact, bool>>>(),
                    It.IsAny<Func<IQueryable<Fact>,
                    IIncludableQueryable<Fact, object>>>()))
            .ReturnsAsync(GetFactWithNotExistingId());

        this._mockMapper
            .Setup(x => x.Map<FactDto?>(It.IsAny<Fact>()))
            .Returns(GetFactDTOWithNotExistingId());

        var expectedError = $"Cannot find any fact with corresponding id: {id}";
        this._mockLocalizerCannotFind.Setup(x => x[It.IsAny<string>(), It.IsAny<object>()]).Returns((string key, object[] args) =>
        {
            if (args != null && args.Length > 0 && args[0] is int id)
            {
                return new LocalizedString(key, $"Cannot find any fact with corresponding id: {id}");
            }

            return new LocalizedString(key, "Cannot find any fact with unknown categoryId");
        });

        var handler = new GetFactByIdHandler(this._mockRepository.Object, this._mockMapper.Object, this._mockLogger.Object, this._mockLocalizerCannotFind.Object);

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
        this._mockRepository
            .Setup(x => x.FactRepository
                .GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<Fact, bool>>>(),
                    It.IsAny<Func<IQueryable<Fact>,
                    IIncludableQueryable<Fact, object>>>()))
            .ReturnsAsync(GetFact(id));

        this._mockMapper
            .Setup(x => x
            .Map<FactDto>(It.IsAny<Fact>()))
            .Returns(GetFactDTO(id));

        var handler = new GetFactByIdHandler(this._mockRepository.Object, this._mockMapper.Object, this._mockLogger.Object, this._mockLocalizerCannotFind.Object);

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
