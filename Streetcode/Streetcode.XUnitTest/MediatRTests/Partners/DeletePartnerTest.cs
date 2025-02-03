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
    private readonly Mock<IMapper> mockMapper;
    private readonly Mock<IRepositoryWrapper> mockRepository;
    private readonly Mock<ILoggerService> mockLogger;
    private readonly Mock<IStringLocalizer<NoSharedResource>> mockLocalizerNoShared;

    public DeletePartnerTest()
    {
        this.mockRepository = new Mock<IRepositoryWrapper>();
        this.mockMapper = new Mock<IMapper>();
        this.mockLogger = new Mock<ILoggerService>();
        this.mockLocalizerNoShared = new Mock<IStringLocalizer<NoSharedResource>>();
    }

    [Fact]
    public async Task ShouldDeleteSuccessfully()
    {
        // Arrange
        var testPartner = GetPartner();

        this.mockMapper.Setup(x => x.Map<PartnerDto>(It.IsAny<Partner>()))
            .Returns(GetPartnerDTO());

        this.mockRepository.Setup(x => x.PartnersRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Partner, bool>>>(), null))
            .ReturnsAsync(testPartner);

        var handler = new DeletePartnerHandler(this.mockRepository.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizerNoShared.Object);

        // Act
        var result = await handler.Handle(new DeletePartnerQuery(testPartner.Id), CancellationToken.None);

        // Assert
        Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.True(result.IsSuccess));

        this.mockRepository.Verify(x => x.PartnersRepository.Delete(It.Is<Partner>(x => x.Id == testPartner.Id)), Times.Once);
        this.mockRepository.Verify(x => x.SaveChanges(), Times.Once);
    }

    [Fact]
    public async Task ShouldThrowExeption_IdNotExisting()
    {
        // Arrange
        var testPartner = GetPartner();
        var expectedError = "No partner with such id";
        this.mockLocalizerNoShared.Setup(x => x["NoPartnerWithSuchId"])
            .Returns(new LocalizedString("NoPartnerWithSuchId", expectedError));

        this.mockRepository.Setup(x => x.PartnersRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Partner, bool>>>(), null))
            .ReturnsAsync(GetPartnerWithNotExistingId());

        // Act
        var handler = new DeletePartnerHandler(this.mockRepository.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizerNoShared.Object);

        var result = await handler.Handle(new DeletePartnerQuery(testPartner.Id), CancellationToken.None);

        // Assert
        Assert.Equal(expectedError, result.Errors[0].Message);

        this.mockRepository.Verify(x => x.PartnersRepository.Delete(It.IsAny<Partner>()), Times.Never);
    }

    [Fact]
    public async Task ShouldThrowExeption_SaveChangesAsyncIsNotSuccessful()
    {
        // Arrange
        var testPartner = GetPartner();
        var expectedError = "The partner wasn`t added";

        this.mockMapper.Setup(x => x.Map<PartnerDto>(It.IsAny<Partner>()))
            .Returns(GetPartnerDTO());

        this.mockRepository.Setup(x => x.PartnersRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Partner, bool>>>(), null))
            .ReturnsAsync(testPartner);
        this.mockRepository.Setup(x => x.SaveChanges())
            .Throws(new Exception(expectedError));

        // Act
        var handler = new DeletePartnerHandler(this.mockRepository.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizerNoShared.Object);

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

    private static PartnerDto GetPartnerDTO()
    {
        return new PartnerDto();
    }

    private static Partner? GetPartnerWithNotExistingId()
    {
        return null;
    }
}
