using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.DTO.Streetcode.Types;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.Fact.GetAll;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.GetById;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.AdditionalContent;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Enums;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Streetcode;

public class GetStreetcodeByIdHandlerTests
{
    private readonly Mock<IRepositoryWrapper> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IStringLocalizer<CannotFindSharedResource>> _mockLocalizerCannotFind;
    private readonly Mock<ILoggerService> _mockLogger;
    private readonly GetStreetcodeByIdHandler _handler;

    public GetStreetcodeByIdHandlerTests()
    {
        _mockRepository = new Mock<IRepositoryWrapper>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILoggerService>();
        _mockLocalizerCannotFind = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        _handler = new GetStreetcodeByIdHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizerCannotFind.Object);
    }

    [Theory]
    [InlineData(1)]
    public async Task Handle_ReturnsSuccess(int id)
    {
        // Arrange
        var testContentDto = new EventStreetcodeDTO();
        var testContent = new StreetcodeContent();

        RepositorySetup(testContent);
        MapperSetup(testContentDto);

        // act
        var result = await _handler.Handle(new GetStreetcodeByIdQuery(id, UserRole.User), CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Theory]
    [InlineData(1)]
    public async Task Handle_ReturnsCorrectType(int id)
    {
        // arrange
        var testContentDto = new EventStreetcodeDTO();
        var testContent = new StreetcodeContent();

        RepositorySetup(testContent);
        MapperSetup(testContentDto);

        // act
        var result = await _handler.Handle(new GetStreetcodeByIdQuery(id, UserRole.User), CancellationToken.None);

        // Assert
        Assert.IsAssignableFrom<StreetcodeDTO>(result.Value);
    }

    private void RepositorySetup(StreetcodeContent? streetcode)
    {
        _mockRepository.Setup(x => x.StreetcodeRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<StreetcodeContent, bool>>?>(), It.IsAny<Func<IQueryable<StreetcodeContent>, IIncludableQueryable<StreetcodeContent, object>>>()))
            .ReturnsAsync(streetcode);
        _mockRepository.Setup(repo => repo.StreetcodeTagIndexRepository.GetAllAsync(
                It.IsAny<Expression<Func<StreetcodeTagIndex, bool>>>(),
                It.IsAny<Func<IQueryable<StreetcodeTagIndex>,
                    IIncludableQueryable<StreetcodeTagIndex, object>>>()))
            .ReturnsAsync(new List<StreetcodeTagIndex>());
    }

    private void MapperSetup(EventStreetcodeDTO? streetcodeDto)
    {
        _mockMapper.Setup(x => x.Map<StreetcodeDTO?>(It.IsAny<object>()))
            .Returns(streetcodeDto);
    }
}