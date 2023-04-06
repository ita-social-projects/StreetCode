using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.MediatR.Partners.GetById;
using Streetcode.DAL.Entities.Partners;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System.Linq.Expressions;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Partners;

public class GetPartnerByIdTest
{
    private Mock<IRepositoryWrapper> _mockRepository;
    private Mock<IMapper> _mockMapper;

    public GetPartnerByIdTest() {
        _mockMapper = new Mock<IMapper>();
        _mockRepository = new Mock<IRepositoryWrapper>();
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

        var handler = new GetPartnerByIdHandler(_mockRepository.Object, _mockMapper.Object);

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

        var handler = new GetPartnerByIdHandler(_mockRepository.Object, _mockMapper.Object);

        //Act
        var result = await handler.Handle(new GetPartnerByIdQuery(testPartner.Id), CancellationToken.None);

        //Assert
        Assert.Multiple(
            () => Assert.True(result.IsFailed),
            () => Assert.Equal(expectedError, result.Errors.First().Message)
        );
    }

    public async Task ShouldReturnSuccesfully_NotNull ()
    {
        //Arrange
        var testPartner = GetPartner();
        var expectedError = $"Cannot find any partner with corresponding id: {testPartner.Id}";

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

        var handler = new GetPartnerByIdHandler(_mockRepository.Object, _mockMapper.Object);

        //Act
        var result = await handler.Handle(new GetPartnerByIdQuery(testPartner.Id), CancellationToken.None);

        //Assert
        Assert.Multiple(
            () => Assert.NotNull(result),
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

        var handler = new GetPartnerByIdHandler(_mockRepository.Object, _mockMapper.Object);

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
