using System.Linq.Expressions;
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
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Partners;

public class GetPartnerByIdTest
{
    private readonly Mock<IRepositoryWrapper> mockRepository;
    private readonly Mock<IMapper> mockMapper;
    private readonly Mock<ILoggerService> mockLogger;
    private readonly Mock<IStringLocalizer<CannotFindSharedResource>> mockLocalizerCannotFind;

    public GetPartnerByIdTest()
    {
        this.mockMapper = new Mock<IMapper>();
        this.mockRepository = new Mock<IRepositoryWrapper>();
        this.mockLogger = new Mock<ILoggerService>();
        this.mockLocalizerCannotFind = new Mock<IStringLocalizer<CannotFindSharedResource>>();
    }

    [Fact]
    public async Task ShouldReturnSuccessfully_ExistingId()
    {
        // Arrange
        var testPartner = GetPartner();

        this.mockRepository.Setup(x => x.PartnersRepository
            .GetSingleOrDefaultAsync(
                It.IsAny<Expression<Func<Partner, bool>>>(),
                It.IsAny<Func<IQueryable<Partner>,
                IIncludableQueryable<Partner, object>>>()))
            .ReturnsAsync(testPartner);

        this.mockMapper
            .Setup(x => x
            .Map<PartnerDTO>(It.IsAny<Partner>()))
            .Returns(GetPartnerDTO());

        var handler = new GetPartnerByIdHandler(this.mockRepository.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizerCannotFind.Object);

        // Act
        var result = await handler.Handle(new GetPartnerByIdQuery(testPartner.Id), CancellationToken.None);

        // Assert
        Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.True(result.IsSuccess),
            () => Assert.Equal(result.Value.Id, testPartner.Id));
    }

    [Fact]
    public async Task ShouldReturnErrorResponse_NotExistingId()
    {
        // Arrange
        var testPartner = GetPartner();
        var expectedError = $"Cannot find any partner with corresponding id: {testPartner.Id}";
        this.mockLocalizerCannotFind.Setup(x => x[It.IsAny<string>(), It.IsAny<object>()]).Returns((string key, object[] args) =>
        {
            if (args != null && args.Length > 0 && args[0] is int)
            {
                return new LocalizedString(key, $"Cannot find any partner with corresponding id: {testPartner.Id}");
            }

            return new LocalizedString(key, "Cannot find any partner with unknown id");
        });

        this.mockRepository.Setup(x => x.PartnersRepository
            .GetSingleOrDefaultAsync(
                It.IsAny<Expression<Func<Partner, bool>>>(),
                It.IsAny<Func<IQueryable<Partner>,
                IIncludableQueryable<Partner, object>>>()))
            .ReturnsAsync(GetPartnerWithNotExistingId());

        this.mockMapper
            .Setup(x => x
            .Map<PartnerDTO?>(It.IsAny<Partner>()))
            .Returns(GetPartnerDTOWithNotExistingId());

        var handler = new GetPartnerByIdHandler(this.mockRepository.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizerCannotFind.Object);

        // Act
        var result = await handler.Handle(new GetPartnerByIdQuery(testPartner.Id), CancellationToken.None);

        // Assert
        Assert.Multiple(
            () => Assert.True(result.IsFailed),
            () => Assert.Equal(expectedError, result.Errors[0].Message));
    }

    [Fact]
    public async Task ShouldReturnSuccessfully_CorrectType()
    {
        // Arrange
        var testPartner = GetPartner();

        this.mockRepository.Setup(x => x.PartnersRepository
            .GetSingleOrDefaultAsync(
                It.IsAny<Expression<Func<Partner, bool>>>(),
                It.IsAny<Func<IQueryable<Partner>,
                IIncludableQueryable<Partner, object>>>()))
            .ReturnsAsync(testPartner);

        this.mockMapper
            .Setup(x => x
            .Map<PartnerDTO>(It.IsAny<Partner>()))
            .Returns(GetPartnerDTO());

        var handler = new GetPartnerByIdHandler(this.mockRepository.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizerCannotFind.Object);

        // Act
        var result = await handler.Handle(new GetPartnerByIdQuery(testPartner.Id), CancellationToken.None);

        // Assert
        Assert.Multiple(
            () => Assert.NotNull(result.ValueOrDefault),
            () => Assert.IsType<PartnerDTO>(result.ValueOrDefault));
    }

    private static Partner GetPartner()
    {
        return new Partner
        {
            Id = 1,
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
            Id = 1,
        };
    }

    private static PartnerDTO? GetPartnerDTOWithNotExistingId()
    {
        return null;
    }
}
