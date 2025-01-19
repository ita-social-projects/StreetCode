using System.Linq.Expressions;
using AutoMapper;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Partners.Create;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Partners;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Partners;

public class CreatePartnerTest
{
    private readonly Mock<IRepositoryWrapper> mockRepository;
    private readonly Mock<IMapper> mockMapper;
    private readonly Mock<ILoggerService> mockLogger;
    private readonly Mock<IStringLocalizer<NoSharedResource>> mockLocalizerNoShared;
    private readonly Mock<IStringLocalizer<FieldNamesSharedResource>> mockLocalizerFieldNames;
    private readonly Mock<IStringLocalizer<AlreadyExistSharedResource>> mockLocalizerAlreadyExist;


    public CreatePartnerTest()
    {
        this.mockRepository = new Mock<IRepositoryWrapper>();
        this.mockMapper = new Mock<IMapper>();
        this.mockLogger = new Mock<ILoggerService>();
        this.mockLocalizerNoShared = new Mock<IStringLocalizer<NoSharedResource>>();
        this.mockLocalizerFieldNames = new Mock<IStringLocalizer<FieldNamesSharedResource>>();
        this.mockLocalizerAlreadyExist = new Mock<IStringLocalizer<AlreadyExistSharedResource>>();
    }

    [Fact]
    public async Task ShouldReturnSuccessfully_TypeIsCorrect()
    {
        // Arrange
        var testPartner = GetPartner();

        this.mockMapper.Setup(x => x.Map<Partner>(It.IsAny<CreatePartnerDTO>()))
            .Returns(testPartner);
        this.mockMapper.Setup(x => x.Map<PartnerDTO>(It.IsAny<Partner>()))
            .Returns(GetPartnerDTO());

        this.mockRepository.Setup(x => x.PartnersRepository.CreateAsync(It.Is<Partner>(y => y.Id == testPartner.Id)))
            .ReturnsAsync(testPartner);
        this.mockRepository.Setup(x => x.StreetcodeRepository.GetAllAsync(It.IsAny<Expression<Func<StreetcodeContent, bool>>>(), null))
            .ReturnsAsync(new List<StreetcodeContent>());
        this.mockRepository.Setup(x => x.SaveChanges())
            .Returns(1);

        var handler = new CreatePartnerHandler(this.mockRepository.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizerNoShared.Object, this.mockLocalizerFieldNames.Object, this.mockLocalizerAlreadyExist.Object);

        // Act
        var result = await handler.Handle(new CreatePartnerQuery(GetCreatePartnerDTO()), CancellationToken.None);

        // Assert
        Assert.IsType<PartnerDTO>(result.Value);
    }

    [Fact]
    public async Task ShouldReturnSuccessfully_WhenPartnerAdded()
    {
        // Arrange
        var testPartner = GetPartner();

        this.mockMapper.Setup(x => x.Map<Partner>(It.IsAny<CreatePartnerDTO>()))
            .Returns(testPartner);
        this.mockMapper.Setup(x => x.Map<PartnerDTO>(It.IsAny<Partner>()))
            .Returns(GetPartnerDTO());

        this.mockRepository.Setup(x => x.PartnersRepository.CreateAsync(It.Is<Partner>(y => y.Id == testPartner.Id)))
            .ReturnsAsync(testPartner);
        this.mockRepository.Setup(x => x.StreetcodeRepository.GetAllAsync(It.IsAny<Expression<Func<StreetcodeContent, bool>>>(), null))
            .ReturnsAsync(new List<StreetcodeContent>());
        this.mockRepository.Setup(x => x.SaveChanges())
            .Returns(1);

        var handler = new CreatePartnerHandler(this.mockRepository.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizerNoShared.Object, this.mockLocalizerFieldNames.Object, this.mockLocalizerAlreadyExist.Object);

        // Act
        var result = await handler.Handle(new CreatePartnerQuery(GetCreatePartnerDTO()), CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task ShouldThrowExeption_SaveChangesIsNotSuccessful()
    {
        // Arrange
        var testPartner = GetPartner();
        var expectedError = "Failed to create a Partner";

        this.mockMapper.Setup(x => x.Map<Partner>(It.IsAny<CreatePartnerDTO>()))
            .Returns(testPartner);
        this.mockMapper.Setup(x => x.Map<PartnerDTO>(It.IsAny<Partner>()))
            .Returns(GetPartnerDTO());

        this.mockRepository.Setup(x => x.PartnersRepository.CreateAsync(It.Is<Partner>(y => y.Id == testPartner.Id)))
            .ReturnsAsync(testPartner);
        this.mockRepository.Setup(x => x.SaveChangesAsync())
            .ThrowsAsync(new Exception(expectedError));

        var handler = new CreatePartnerHandler(this.mockRepository.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizerNoShared.Object, this.mockLocalizerFieldNames.Object, this.mockLocalizerAlreadyExist.Object);

        // Act
        var result = await handler.Handle(new CreatePartnerQuery(GetCreatePartnerDTO()), CancellationToken.None);

        // Assert
        Assert.Equal(expectedError, result.Errors[0].Message);

        this.mockRepository.Verify(x => x.StreetcodeRepository.GetAllAsync(It.IsAny<Expression<Func<StreetcodeContent, bool>>>(), null), Times.Never);
    }

    private static Partner GetPartner()
    {
        return new Partner()
        {
            Id = 1,
        };
    }

    private static PartnerDTO GetPartnerDTO()
    {
        return new PartnerDTO();
    }

    private static CreatePartnerDTO GetCreatePartnerDTO()
    {
        return new CreatePartnerDTO();
    }
}
