using System.Linq.Expressions;
using AutoMapper;
using Moq;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Partners.Create;
using Streetcode.DAL.Entities.Partners;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Partners;

public class CreatePartnerTest
{
    private readonly Mock<IRepositoryWrapper> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILoggerService> _mockLogger;

    public CreatePartnerTest()
    {
        this._mockRepository = new Mock<IRepositoryWrapper>();
        this._mockMapper = new Mock<IMapper>();
        this._mockLogger = new Mock<ILoggerService>();
    }

    [Fact]
    public async Task ShouldReturnSuccessfully_TypeIsCorrect()
    {
        // Arrange
        var testPartner = GetPartner();

        this._mockMapper.Setup(x => x.Map<Partner>(It.IsAny<CreatePartnerDTO>()))
            .Returns(testPartner);
        this._mockMapper.Setup(x => x.Map<PartnerDTO>(It.IsAny<Partner>()))
            .Returns(GetPartnerDTO());

        this._mockRepository.Setup(x => x.PartnersRepository.CreateAsync(It.Is<Partner>(y => y.Id == testPartner.Id)))
            .ReturnsAsync(testPartner);
        this._mockRepository.Setup(x => x.StreetcodeRepository.GetAllAsync(It.IsAny<Expression<Func<StreetcodeContent, bool>>>(), null))
            .ReturnsAsync(new List<StreetcodeContent>());
        this._mockRepository.Setup(x => x.SaveChanges())
            .Returns(1);

        var handler = new CreatePartnerHandler(this._mockRepository.Object, this._mockMapper.Object, this._mockLogger.Object);

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

        this._mockMapper.Setup(x => x.Map<Partner>(It.IsAny<CreatePartnerDTO>()))
            .Returns(testPartner);
        this._mockMapper.Setup(x => x.Map<PartnerDTO>(It.IsAny<Partner>()))
            .Returns(GetPartnerDTO());

        this._mockRepository.Setup(x => x.PartnersRepository.CreateAsync(It.Is<Partner>(y => y.Id == testPartner.Id)))
            .ReturnsAsync(testPartner);
        this._mockRepository.Setup(x => x.StreetcodeRepository.GetAllAsync(It.IsAny<Expression<Func<StreetcodeContent, bool>>>(), null))
            .ReturnsAsync(new List<StreetcodeContent>());
        this._mockRepository.Setup(x => x.SaveChanges())
            .Returns(1);

        var handler = new CreatePartnerHandler(this._mockRepository.Object, this._mockMapper.Object, this._mockLogger.Object);

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

        this._mockMapper.Setup(x => x.Map<Partner>(It.IsAny<CreatePartnerDTO>()))
            .Returns(testPartner);
        this._mockMapper.Setup(x => x.Map<PartnerDTO>(It.IsAny<Partner>()))
            .Returns(GetPartnerDTO());

        this._mockRepository.Setup(x => x.PartnersRepository.CreateAsync(It.Is<Partner>(y => y.Id == testPartner.Id)))
            .ReturnsAsync(testPartner);

        this._mockRepository.Setup(x => x.SaveChanges())
            .Throws(new Exception(expectedError));

        var handler = new CreatePartnerHandler(this._mockRepository.Object, this._mockMapper.Object, this._mockLogger.Object);

        // Act
        var result = await handler.Handle(new CreatePartnerQuery(GetCreatePartnerDTO()), CancellationToken.None);

        // Assert
        Assert.Equal(expectedError, result.Errors[0].Message);

        this._mockRepository.Verify(x => x.StreetcodeRepository.GetAllAsync(It.IsAny<Expression<Func<StreetcodeContent, bool>>>(), null), Times.Never);
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
