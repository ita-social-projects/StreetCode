using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Partners.GetById;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Partners;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System.Linq.Expressions;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Partners;

public class GetPartnerByIdTest
{
    private readonly Mock<IRepositoryWrapper> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILoggerService> _mockLogger;
    private readonly Mock<IStringLocalizer<CannotFindSharedResource>> _mockLocalizerCannotFind;

    public GetPartnerByIdTest() {
        _mockMapper = new Mock<IMapper>();
        _mockRepository = new Mock<IRepositoryWrapper>();
        _mockLogger = new Mock<ILoggerService>();
        _mockLocalizerCannotFind = new Mock<IStringLocalizer<CannotFindSharedResource>>();
    }

    [Fact]
    public async Task ShouldReturnSuccessfully_ExistingId()
    {
        //Arrange
        var testPartner = GetPartner();

        _mockRepository.Setup(x => x.PartnersRepository
            .GetSingleOrDefaultAsync(
               It.IsAny<Expression<Func<Partner, bool>>>(),
                It.IsAny<Func<IQueryable<Partner>,
                IIncludableQueryable<Partner, object>>>()))
            .ReturnsAsync(testPartner);

        _mockMapper
            .Setup(x => x
            .Map<PartnerDTO>(It.IsAny<Partner>()))
            .Returns(GetPartnerDTO());

        var handler = new GetPartnerByIdHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizerCannotFind.Object);

        //Act
        var result = await handler.Handle(new GetPartnerByIdQuery(testPartner.Id), CancellationToken.None);

        //Assert
        Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.True(result.IsSuccess),
            () => Assert.Equal(result.Value.Id, testPartner.Id)
        );
    }

    [Fact]
    public async Task ShouldReturnErrorResponse_NotExistingId()
    {
        //Arrange
        var testPartner = GetPartner();
        var expectedError = $"Cannot find any partner with corresponding id: {testPartner.Id}";
        _mockLocalizerCannotFind.Setup(x => x[It.IsAny<string>(), It.IsAny<object>()]).Returns((string key, object[] args) =>
        {
            if (args != null && args.Length > 0 && args[0] is int id)
            {
                return new LocalizedString(key, $"Cannot find any partner with corresponding id: {testPartner.Id}");
            }

            return new LocalizedString(key, "Cannot find any partner with unknown id");
        });

        _mockRepository.Setup(x => x.PartnersRepository
            .GetSingleOrDefaultAsync(
               It.IsAny<Expression<Func<Partner, bool>>>(),
                It.IsAny<Func<IQueryable<Partner>,
                IIncludableQueryable<Partner, object>>>()))
            .ReturnsAsync(GetPartnerWithNotExistingId());

        _mockMapper
            .Setup(x => x
            .Map<PartnerDTO>(It.IsAny<Partner>()))
            .Returns(GetPartnerDTOWithNotExistingId());

        var handler = new GetPartnerByIdHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizerCannotFind.Object);

        //Act
        var result = await handler.Handle(new GetPartnerByIdQuery(testPartner.Id), CancellationToken.None);

        //Assert
        Assert.Multiple(
            () => Assert.True(result.IsFailed),
            () => Assert.Equal(expectedError, result.Errors.First().Message)
        );
    }

    [Fact]
    public async Task ShouldReturnSuccessfully_CorrectType()
    {
        //Arrange
        var testPartner = GetPartner();

        _mockRepository.Setup(x => x.PartnersRepository
            .GetSingleOrDefaultAsync(
               It.IsAny<Expression<Func<Partner, bool>>>(),
                It.IsAny<Func<IQueryable<Partner>,
                IIncludableQueryable<Partner, object>>>()))
            .ReturnsAsync(testPartner);

        _mockMapper
            .Setup(x => x
            .Map<PartnerDTO>(It.IsAny<Partner>()))
            .Returns(GetPartnerDTO());

        var handler = new GetPartnerByIdHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizerCannotFind.Object);

        //Act
        var result = await handler.Handle(new GetPartnerByIdQuery(testPartner.Id), CancellationToken.None);

        //Assert
        Assert.Multiple(
            () => Assert.NotNull(result.ValueOrDefault),
            () => Assert.IsType<PartnerDTO>(result.ValueOrDefault)
        );
    }

    private static Partner GetPartner()
    {
        return new Partner
        {
            Id = 1
        };
    }
    private static Partner? GetPartnerWithNotExistingId()
    {
        return null;
    }

    private static PartnerDTO GetPartnerDTO()
    {
        return new PartnerDTO
        {
            Id = 1
        };
    }

    private static PartnerDTO? GetPartnerDTOWithNotExistingId()
    {
        return null;
    }
}
