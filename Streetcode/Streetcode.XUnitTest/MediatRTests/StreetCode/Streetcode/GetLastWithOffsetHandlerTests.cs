using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.GetLastWithOffset;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Streetcode;

public class GetLastWithOffsetHandlerTests
{
    private readonly Mock<IRepositoryWrapper> _repository;
    private readonly Mock<IMapper> _mapper; 
    private readonly Mock<ILoggerService> _mockLogger;
    private readonly Mock<IStringLocalizer<NoSharedResource>> _mockLocalizerCannotFind;

    public GetLastWithOffsetHandlerTests()
        {
            _repository = new Mock<IRepositoryWrapper>();
            _mapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILoggerService>();
            _mockLocalizerCannotFind = new Mock<IStringLocalizer<NoSharedResource>>();
        }

    [Theory]
    [InlineData(0)]
    public async Task HandleReturnsSuccess(int offset)
    {
        var testContentDTO = new StreetcodeMainPageDTO();
        var testContent = new StreetcodeContent();

        RepositorySetup(testContent);
        MapperSetup(testContentDTO);

        var handler = new GetLastWithOffsetHandler(_repository.Object, _mapper.Object, _mockLogger.Object, _mockLocalizerCannotFind.Object);

        var result = await handler.Handle(new GetLastWithOffsetQuery(offset), CancellationToken.None);

        Assert.True(result.IsSuccess);
    }

    [Theory]
    [InlineData(0)]
    public async Task HandleReturnsCorrectType(int offset)
    {
        var testContentDTO = new StreetcodeMainPageDTO();
        var testContent = new StreetcodeContent();

        RepositorySetup(testContent);
        MapperSetup(testContentDTO);

        var handler = new GetLastWithOffsetHandler(_repository.Object, _mapper.Object, _mockLogger.Object, _mockLocalizerCannotFind.Object);
        var result = await handler.Handle(new GetLastWithOffsetQuery(offset), CancellationToken.None);
        Assert.IsAssignableFrom<StreetcodeMainPageDTO>(result.Value);
    }

    [Theory]
    [InlineData(0)]
    public async Task HandleReturnsError(int offset)
    {
        string expectedErrorMessage = "No streetcodes exist now";
        var mockLocalizedErrorString = new LocalizedString("NoStreetcodesExistNow", "No streetcodes exist now");
        _mockLocalizerCannotFind.Setup(x => x["NoStreetcodesExistNow"])
            .Returns(mockLocalizedErrorString);

        RepositorySetup(null);
        MapperSetup(null);

        var handler = new GetLastWithOffsetHandler(_repository.Object, _mapper.Object, _mockLogger.Object, _mockLocalizerCannotFind.Object);
        var result = await handler.Handle(new GetLastWithOffsetQuery(offset), CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Single(result.Errors);
        Assert.Equal(expectedErrorMessage, result.Errors.First().Message);
    }

    private void RepositorySetup(StreetcodeContent streetcode)
    {
        _repository.Setup(x => x.StreetcodeRepository.GetFirstOrDefaultAsync(
            It.IsAny<Expression<Func<StreetcodeContent, StreetcodeContent>>>(),
            It.IsAny<Expression<Func<StreetcodeContent, bool>>>(),
            It.IsAny<Func<IQueryable<StreetcodeContent>, IIncludableQueryable<StreetcodeContent, object>>>(), // include
            It.IsAny<Expression<Func<StreetcodeContent, object>>>(),
            It.IsAny<Expression<Func<StreetcodeContent, object>>>(),
            It.IsAny<int?>()
        )).ReturnsAsync(streetcode);
    }

    private void MapperSetup(StreetcodeMainPageDTO streetcodeDTO)
    {
        _mapper.Setup(x => x.Map<StreetcodeMainPageDTO>(It.IsAny<object>()))
            .Returns(streetcodeDTO);
    }
}