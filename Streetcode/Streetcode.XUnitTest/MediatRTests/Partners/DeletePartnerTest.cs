using System.Linq.Expressions;
using AutoMapper;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Partners.Delete;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Partners;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Partners;

public class DeletePartnerTest
{
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IRepositoryWrapper> _mockRepository;
    private readonly Mock<ILoggerService> _mockLogger;
    private readonly Mock<IStringLocalizer<NoSharedResource>> _mockLocalizerNoShared;

    public DeletePartnerTest()
    {
        this._mockRepository = new Mock<IRepositoryWrapper>();
        this._mockMapper = new Mock<IMapper>();
        this._mockLogger = new Mock<ILoggerService>();
        this._mockLocalizerNoShared = new Mock<IStringLocalizer<NoSharedResource>>();
    }

    [Fact]
    public async Task ShouldDeleteSuccessfully()
    {
        // Arrange
        var testPartner = GetPartner();

        this._mockMapper.Setup(x => x.Map<PartnerDTO>(It.IsAny<Partner>()))
            .Returns(GetPartnerDTO());

        this._mockRepository.Setup(x => x.PartnersRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Partner, bool>>>(), null))
            .ReturnsAsync(testPartner);

        var handler = new DeletePartnerHandler(this._mockRepository.Object, this._mockMapper.Object, this._mockLogger.Object, this._mockLocalizerNoShared.Object);

        // Act
        var result = await handler.Handle(new DeletePartnerQuery(testPartner.Id), CancellationToken.None);

        // Assert
        Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.True(result.IsSuccess));

        this._mockRepository.Verify(x => x.PartnersRepository.Delete(It.Is<Partner>(x => x.Id == testPartner.Id)), Times.Once);
        this._mockRepository.Verify(x => x.SaveChanges(), Times.Once);
    }

    [Fact]
    public async Task ShouldThrowExeption_IdNotExisting()
    {
        // Arrange
        var testPartner = GetPartner();
        var expectedError = "No partner with such id";
        this._mockLocalizerNoShared.Setup(x => x["NoPartnerWithSuchId"])
            .Returns(new LocalizedString("NoPartnerWithSuchId", expectedError));

        this._mockRepository.Setup(x => x.PartnersRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Partner, bool>>>(), null))
            .ReturnsAsync(GetPartnerWithNotExistingId());

        // Act
        var handler = new DeletePartnerHandler(this._mockRepository.Object, this._mockMapper.Object, this._mockLogger.Object, this._mockLocalizerNoShared.Object);

        var result = await handler.Handle(new DeletePartnerQuery(testPartner.Id), CancellationToken.None);

        // Assert
        Assert.Equal(expectedError, result.Errors[0].Message);

        this._mockRepository.Verify(x => x.PartnersRepository.Delete(It.IsAny<Partner>()), Times.Never);
    }

    [Fact]
    public async Task ShouldThrowExeption_SaveChangesAsyncIsNotSuccessful()
    {
        // Arrange
        var testPartner = GetPartner();
        var expectedError = "The partner wasn`t added";

        this._mockMapper.Setup(x => x.Map<PartnerDTO>(It.IsAny<Partner>()))
            .Returns(GetPartnerDTO());

        this._mockRepository.Setup(x => x.PartnersRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Partner, bool>>>(), null))
            .ReturnsAsync(testPartner);
        this._mockRepository.Setup(x => x.SaveChanges())
            .Throws(new Exception(expectedError));

        // Act
        var handler = new DeletePartnerHandler(this._mockRepository.Object, this._mockMapper.Object, this._mockLogger.Object, this._mockLocalizerNoShared.Object);

        var result = await handler.Handle(new DeletePartnerQuery(testPartner.Id), CancellationToken.None);

        // Assert
        Assert.Equal(expectedError, result.Errors[0].Message);
    }

    private static Partner GetPartner()
    {
        return new Partner
        {
            Id = 1,
        };
    }

    private static PartnerDTO GetPartnerDTO()
    {
        return new PartnerDTO();
    }

    private static Partner? GetPartnerWithNotExistingId()
    {
        return null;
    }
}
