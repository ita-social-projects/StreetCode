using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Streetcode.TextContent.Fact;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.Fact.GetByStreetcodeId;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Facts;

public class GetFactByStreetcodeIdTest
{
    private readonly Mock<IRepositoryWrapper> mockRepository;
    private readonly Mock<IMapper> mockMapper;
    private readonly Mock<ILoggerService> mockLogger;
    private readonly Mock<IStringLocalizer<CannotFindSharedResource>> mockLocalizerCannotFind;

    public GetFactByStreetcodeIdTest()
    {
        this.mockRepository = new Mock<IRepositoryWrapper>();
        this.mockMapper = new Mock<IMapper>();
        this.mockLogger = new Mock<ILoggerService>();
        this.mockLocalizerCannotFind = new Mock<IStringLocalizer<CannotFindSharedResource>>();
    }

    [Theory]
    [InlineData(1)]
    public async Task ShouldReturnSuccessfully_ExistingId(int streetCodeId)
    {
        // Arrange
        this.mockRepository.Setup(x => x.FactRepository
            .GetAllAsync(
                It.IsAny<Expression<Func<Fact, bool>>>(),
                It.IsAny<Func<IQueryable<Fact>,
                IIncludableQueryable<Fact, object>>>()))
            .ReturnsAsync(GetListFacts());

        this.mockMapper
            .Setup(x => x
            .Map<IEnumerable<FactDto>>(It.IsAny<IEnumerable<Fact>>()))
            .Returns(GetListFactDTO());

        var handler = new GetFactByStreetcodeIdHandler(this.mockRepository.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizerCannotFind.Object);

        // Act
        var result = await handler.Handle(new GetFactByStreetcodeIdQuery(streetCodeId), CancellationToken.None);

        // Assert
        Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.True(result.IsSuccess),
            () => Assert.NotEmpty(result.Value));
    }

    [Theory]
    [InlineData(2)]
    public async Task ShouldReturnSuccessfully_CorrectType(int streetCodeId)
    {
        // Arrange
        this.mockRepository.Setup(x => x.FactRepository
            .GetAllAsync(
                It.IsAny<Expression<Func<Fact, bool>>>(),
                It.IsAny<Func<IQueryable<Fact>,
                IIncludableQueryable<Fact, object>>>()))
            .ReturnsAsync(GetListFacts());

        this.mockMapper
            .Setup(x => x
            .Map<IEnumerable<FactDto>>(It.IsAny<IEnumerable<Fact>>()))
            .Returns(GetListFactDTO());

        var handler = new GetFactByStreetcodeIdHandler(this.mockRepository.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizerCannotFind.Object);

        // Act
        var result = await handler.Handle(new GetFactByStreetcodeIdQuery(streetCodeId), CancellationToken.None);

        // Assert
        Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.True(result.IsSuccess),
            () => Assert.IsType<List<FactDto>>(result.ValueOrDefault));
    }

    [Theory]
    [InlineData(1)]
    public async Task ShouldThrowError_IdNotExist(int streetCodeId)
    {
        // Arrange
        this.mockRepository.Setup(x => x.FactRepository
            .GetAllAsync(
                It.IsAny<Expression<Func<Fact, bool>>>(),
                It.IsAny<Func<IQueryable<Fact>,
                IIncludableQueryable<Fact, object>>>()))
            .ReturnsAsync(GetListFactsWithNotExistingStreetcodeId() !);

        this.mockMapper
            .Setup(x => x
            .Map<IEnumerable<FactDto>>(It.IsAny<IEnumerable<Fact>>()))
            .Returns(GetListFactsDTOWithNotExistingId() !);

        var expectedError = $"Cannot find any fact by the streetcode id: {streetCodeId}";
        this.mockLocalizerCannotFind.Setup(x => x[It.IsAny<string>(), It.IsAny<object>()]).Returns((string key, object[] args) =>
        {
            if (args != null && args.Length > 0 && args[0] is int streetCodeId)
            {
                return new LocalizedString(key, $"Cannot find any fact by the streetcode id: {streetCodeId}");
            }

            return new LocalizedString(key, "Cannot find any fact with unknown categoryId");
        });

        var handler = new GetFactByStreetcodeIdHandler(this.mockRepository.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizerCannotFind.Object);

        // Act
        var result = await handler.Handle(new GetFactByStreetcodeIdQuery(streetCodeId), CancellationToken.None);

        // Assert
        Assert.Multiple(
            () => Assert.True(result.IsFailed),
            () => Assert.Equal(expectedError, result.Errors[0].Message));
    }

    private static IQueryable<Fact> GetListFacts()
    {
        var facts = new List<Fact>
        {
            new Fact
            {
                Id = 1,
                Title = "Викуп з кріпацтва",
                ImageId = null,
                Streetcode = new StreetcodeContent
                {
                    Id = 1,
                },
                StreetcodeId = 1,
                FactContent = "Навесні 1838-го Карл Брюллов...",
            },
        };

        return facts.AsQueryable();
    }

    private static List<Fact>? GetListFactsWithNotExistingStreetcodeId()
    {
        return null;
    }

    private static List<FactDto>? GetListFactsDTOWithNotExistingId()
    {
        return null;
    }

    private static List<FactDto> GetListFactDTO()
    {
        var facts = new List<FactDto>
        {
            new FactDto
            {
                Id = 1,
            },
        };

        return facts;
    }
}