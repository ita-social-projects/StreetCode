using System.Linq.Expressions;
using AutoMapper;
using FluentResults;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using MockQueryable.Moq;
using Moq;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Partners.GetByStreetcodeId;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Partners;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Enums;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Partners;

public class GetParnerByStreetcodeIdTest
{
    private readonly Mock<IRepositoryWrapper> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILoggerService> _mockLogger;
    private readonly MockCannotFindLocalizer _mockLocalizerCannotFind;

    public GetParnerByStreetcodeIdTest()
    {
        _mockRepository = new Mock<IRepositoryWrapper>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILoggerService>();
        _mockLocalizerCannotFind = new MockCannotFindLocalizer();
    }

    [Fact]
    public async Task ShouldReturnSuccessfully_ExistingIdAndUserHasAccess()
    {
        // Arrange
        var testStreetcodeContent = GetStreetcodeList()[0];

        SetupRepositoryMock(GetPartnerList(), GetStreetcodeList());

        _mockMapper
            .Setup(x => x
            .Map<IEnumerable<PartnerDTO>>(It.IsAny<IEnumerable<Partner>>()))
            .Returns(GetPartnerDTOList());

        var handler = new GetPartnersByStreetcodeIdHandler(_mockMapper.Object, _mockRepository.Object, _mockLogger.Object, _mockLocalizerCannotFind);

        // Act
        var result = await handler.Handle(new GetPartnersByStreetcodeIdQuery(testStreetcodeContent.Id, UserRole.User), CancellationToken.None);

        // Assert
        Assert.Multiple(
            () => Assert.True(result.IsSuccess),
            () => Assert.NotEmpty(result.Value));
    }

    [Fact]
    public async Task Handler_PartnerExistsButUserDoesNotHaveAccess_ReturnsError()
    {
        // Arrange
        var testStreetcodeContent = GetStreetcodeList()[0];

        SetupRepositoryMock(GetPartnerList(), new List<StreetcodeContent>());

        var expectedError = _mockLocalizerCannotFind["CannotFindAnyStreetcodeWithCorrespondingId", testStreetcodeContent.Id].Value;

        _mockMapper
            .Setup(x => x.Map<IEnumerable<PartnerDTO>>(It.IsAny<IEnumerable<Partner>>()))
            .Returns(GetPartnerDTOList());

        var handler = new GetPartnersByStreetcodeIdHandler(_mockMapper.Object, _mockRepository.Object, _mockLogger.Object, _mockLocalizerCannotFind);

        // Act
        var result = await handler.Handle(new GetPartnersByStreetcodeIdQuery(testStreetcodeContent.Id, UserRole.User), CancellationToken.None);

        // Assert
        Assert.Equal(expectedError, result.Errors.Single().Message);
    }

    [Fact]
    public async Task ShouldReturnSuccessfully_CorrectType()
    {
        // Arrange
        var testStreetcodeContent = GetStreetcodeList()[0];

        SetupRepositoryMock(GetPartnerList(), GetStreetcodeList());

        _mockMapper
            .Setup(x => x
            .Map<IEnumerable<PartnerDTO>>(It.IsAny<IEnumerable<Partner>>()))
            .Returns(GetPartnerDTOList());

        var handler = new GetPartnersByStreetcodeIdHandler(_mockMapper.Object, _mockRepository.Object, _mockLogger.Object, _mockLocalizerCannotFind);

        // Act
        var result = await handler.Handle(new GetPartnersByStreetcodeIdQuery(testStreetcodeContent.Id, UserRole.User), CancellationToken.None);

        // Assert
        Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.True(result.IsSuccess),
            () => Assert.IsType<List<PartnerDTO>>(result.ValueOrDefault));
    }

    [Fact]
    public async Task ShouldReturnSuccessfully_EmptyListOfPartnersAndUserHasAccess()
    {
        // Arrange
        var testStreetcodeContent = GetStreetcodeList()[0];

        SetupRepositoryMock(new List<Partner>(), GetStreetcodeList());

        var handler = new GetPartnersByStreetcodeIdHandler(_mockMapper.Object, _mockRepository.Object, _mockLogger.Object, _mockLocalizerCannotFind);

        // Act
        var result = await handler.Handle(new GetPartnersByStreetcodeIdQuery(testStreetcodeContent.Id, UserRole.User), CancellationToken.None);

        // Asset
        Assert.Multiple(
                () => Assert.IsType<Result<IEnumerable<PartnerDTO>>>(result),
                () => Assert.IsAssignableFrom<IEnumerable<PartnerDTO>>(result.Value),
                () => Assert.Empty(result.Value));
    }

    private void SetupRepositoryMock(IEnumerable<Partner> partners, List<StreetcodeContent> streetcodeListUserCanAccess)
    {
        _mockRepository.Setup(x => x.PartnersRepository
                .GetAllAsync(
                    It.IsAny<Expression<Func<Partner, bool>>>(),
                    It.IsAny<Func<IQueryable<Partner>,
                        IIncludableQueryable<Partner, object>>>()))
            .ReturnsAsync(partners);

        _mockRepository.Setup(repo => repo.StreetcodeRepository
                .FindAll(
                    It.IsAny<Expression<Func<StreetcodeContent, bool>>>(),
                    It.IsAny<Func<IQueryable<StreetcodeContent>,
                        IIncludableQueryable<StreetcodeContent, object>>>()))
            .Returns(streetcodeListUserCanAccess.AsQueryable().BuildMockDbSet().Object);
    }

    private static IEnumerable<Partner> GetPartnerList()
    {
        var partners = new List<Partner>
        {
            new Partner
            {
                Id = 1,
                Streetcodes = GetStreetcodeList(),
            },
        };

        return partners;
    }

    private static List<StreetcodeContent> GetStreetcodeList()
    {
        var streetCodes = new List<StreetcodeContent>
        {
            new StreetcodeContent
            {
                Id = 1,
            },
        };

        return streetCodes;
    }

    private static List<PartnerDTO> GetPartnerDTOList()
    {
        var partners = new List<PartnerDTO>
        {
            new PartnerDTO
            {
                Id = 1,
            },
        };

        return partners;
    }
}