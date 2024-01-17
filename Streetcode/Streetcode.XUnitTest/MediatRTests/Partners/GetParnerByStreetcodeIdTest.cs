using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Partners.GetByStreetcodeId;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Partners;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System.Linq.Expressions;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Partners;

public class GetParnerByStreetcodeIdTest
{
    private readonly Mock<IRepositoryWrapper> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILoggerService> _mockLogger;
    private readonly Mock<IStringLocalizer<CannotFindSharedResource>> _mockLocalizerCannotFind;


    public GetParnerByStreetcodeIdTest()
    {
        _mockRepository = new Mock<IRepositoryWrapper>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILoggerService>();
        _mockLocalizerCannotFind = new Mock<IStringLocalizer<CannotFindSharedResource>>();
    }

    [Fact]
    public async Task ShouldReturnSuccessfully_ExistingId()
    {
        //Arrange
        var testStreetcodeContent = GetStreetcodeList().First();

        _mockRepository.Setup(x => x.StreetcodeRepository
            .GetSingleOrDefaultAsync(
                It.IsAny<Expression<Func<StreetcodeContent, bool>>>(),
                null))
            .ReturnsAsync(testStreetcodeContent);

        _mockRepository.Setup(x => x.PartnersRepository
              .GetAllAsync(
                  It.IsAny<Expression<Func<Partner, bool>>>(),
                    It.IsAny<Func<IQueryable<Partner>,
              IIncludableQueryable<Partner, object>>>()))
              .ReturnsAsync(GetPartnerList());

        _mockMapper
            .Setup(x => x
            .Map<IEnumerable<PartnerDTO>>(It.IsAny<IEnumerable<Partner>>()))
            .Returns(GetPartnerDTOList());

        var handler = new GetPartnersByStreetcodeIdHandler(_mockMapper.Object, _mockRepository.Object, _mockLogger.Object, _mockLocalizerCannotFind.Object);

        //Act
        var result = await handler.Handle(new GetPartnersByStreetcodeIdQuery(testStreetcodeContent.Id), CancellationToken.None);

        //Assert
        Assert.Multiple(
            () => Assert.True(result.IsSuccess),
            () => Assert.NotEmpty(result.Value)
        );
    }

    [Fact]
    public async Task ShouldReturnSuccessfully_CorrectType()
    {
        //Arrange
        var testStreetcodeContent = GetStreetcodeList().First();

        _mockRepository.Setup(x => x.StreetcodeRepository
            .GetSingleOrDefaultAsync(
                It.IsAny<Expression<Func<StreetcodeContent, bool>>>(),
                null))
            .ReturnsAsync(testStreetcodeContent);

        _mockRepository.Setup(x => x.PartnersRepository
              .GetAllAsync(
                  It.IsAny<Expression<Func<Partner, bool>>>(),
                    It.IsAny<Func<IQueryable<Partner>,
              IIncludableQueryable<Partner, object>>>()))
              .ReturnsAsync(GetPartnerList());

        _mockMapper
            .Setup(x => x
            .Map<IEnumerable<PartnerDTO>>(It.IsAny<IEnumerable<Partner>>()))
            .Returns(GetPartnerDTOList());

        var handler = new GetPartnersByStreetcodeIdHandler(_mockMapper.Object, _mockRepository.Object, _mockLogger.Object, _mockLocalizerCannotFind.Object);

        //Act
        var result = await handler.Handle(new GetPartnersByStreetcodeIdQuery(testStreetcodeContent.Id), CancellationToken.None);

        //Assert
        Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.True(result.IsSuccess),
            () => Assert.IsType<List<PartnerDTO>>(result.ValueOrDefault)
        );
    }

    [Fact]
    public async Task ShouldThrowError_IdNotExist()
    {
        //Arrange
        var testStreetcodeContent = GetStreetcodeList().First();
        var expectedError = $"Cannot find a partners by a streetcode id: {testStreetcodeContent.Id}";
        _mockLocalizerCannotFind.Setup(x => x[It.IsAny<string>(), It.IsAny<object>()]).Returns((string key, object[] args) =>
        {
            if (args != null && args.Length > 0 && args[0] is int id)
            {
                return new LocalizedString(key, $"Cannot find a partners by a streetcode id: {testStreetcodeContent.Id}");
            }

            return new LocalizedString(key, "Cannot find any partners with unknown Id");
        });
            _mockRepository.Setup(x => x.StreetcodeRepository
            .GetSingleOrDefaultAsync(
                It.IsAny<Expression<Func<StreetcodeContent, bool>>>(),
                null))
            .ReturnsAsync(testStreetcodeContent);

        _mockRepository.Setup(x => x.PartnersRepository
              .GetAllAsync(
                  It.IsAny<Expression<Func<Partner, bool>>>(),
                    It.IsAny<Func<IQueryable<Partner>,
              IIncludableQueryable<Partner, object>>>()))
              .ReturnsAsync(GetPartnerListWithNotExistingStreetcodeId());

        _mockMapper
            .Setup(x => x
            .Map<IEnumerable<PartnerDTO>>(It.IsAny<IEnumerable<Partner>>()))
            .Returns(GetPartnerDTOListWithNotExistingId());

        var handler = new GetPartnersByStreetcodeIdHandler(_mockMapper.Object, _mockRepository.Object, _mockLogger.Object, _mockLocalizerCannotFind.Object);
    
        //Act
        var result = await handler.Handle(new GetPartnersByStreetcodeIdQuery(testStreetcodeContent.Id), CancellationToken.None);

        //Assert
        Assert.Multiple(
            () => Assert.True(result.IsFailed),
            () => Assert.Equal(expectedError, result.Errors.First().Message)
        );
    }

    private static IEnumerable<Partner> GetPartnerList()
    {
        var partners = new List<Partner>
        {
            new Partner
            {
                Id = 1,
                Streetcodes = GetStreetcodeList()
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
                Id = 1
            },
        };

        return streetCodes;
    }
    private static List<Partner>? GetPartnerListWithNotExistingStreetcodeId()
    {
        return null;
    }
    private static List<PartnerDTO>? GetPartnerDTOListWithNotExistingId()
    {
        return null;
    }
    private static List<PartnerDTO> GetPartnerDTOList()
    {
        var partners = new List<PartnerDTO>
        {
            new PartnerDTO
            {
                Id = 1
            },
        };

        return partners;
    }
}
