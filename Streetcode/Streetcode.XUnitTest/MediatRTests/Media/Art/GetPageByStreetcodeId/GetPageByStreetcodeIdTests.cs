using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Media.Art;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Media.StreetcodeArt.GetByStreetcodeId;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Media.Art.GetPageByStreetcodeId;

public class GetPageByStreetcodeIdTests
{
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IRepositoryWrapper> _mockRepositoryWrapper;
    private readonly Mock<IBlobService> _mockBlobService;
    private readonly Mock<ILoggerService> _mockLogger;
    private readonly Mock<IStringLocalizer<CannotFindSharedResource>> _mockStringLocalizerCannotFind;

    public GetPageByStreetcodeIdTests()
    {
        _mockMapper = new Mock<IMapper>();
        _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
        _mockBlobService = new Mock<IBlobService>();
        _mockLogger = new Mock<ILoggerService>();
        _mockStringLocalizerCannotFind = new Mock<IStringLocalizer<CannotFindSharedResource>>();
    }

    [Fact]
    public async Task GetPageByStreetcodeId_ShouldReturnPageByStreetcodeId()
    {
        List<StreetcodeArt> streetcodeArts = new List<StreetcodeArt>();

        SetupRepositoryWrapper(streetcodeArts);
        SetupMapper();
        SetupBlobService();

        var handler = new GetStreetcodeArtByStreetcodeIdHandler(_mockRepositoryWrapper.Object, _mockMapper.Object,
            _mockBlobService.Object, _mockLogger.Object, _mockStringLocalizerCannotFind.Object);

        var response = await handler.Handle(new GetStreetcodeArtByStreetcodeIdQuery(1), CancellationToken.None);

        Assert.True(response.IsSuccess);
    }

    [Fact]
    public async Task GetPageByStreetcodeId_ShouldReturnFailure()
    {
        int invalidId = -1;

        SetupRepositoryWrapper(null);
        SetupMapper();
        SetupBlobService();
        SetupStringLocalizer(invalidId, "Cannot find any art with corresponding streetcode id");

        var handler = new GetStreetcodeArtByStreetcodeIdHandler(_mockRepositoryWrapper.Object, _mockMapper.Object,
            _mockBlobService.Object, _mockLogger.Object, _mockStringLocalizerCannotFind.Object);

        var response = await handler.Handle(new GetStreetcodeArtByStreetcodeIdQuery(invalidId), CancellationToken.None);

        Assert.True(response.IsFailed);
    }

    private void SetupRepositoryWrapper(IEnumerable<StreetcodeArt> streetcodeArts)
    {
        _mockRepositoryWrapper.Setup(repo => repo.StreetcodeArtRepository
                .GetAllAsync(
                    It.IsAny<Expression<Func<StreetcodeArt, bool>>>(),
                    It.IsAny<Func<IQueryable<StreetcodeArt>, IIncludableQueryable<StreetcodeArt, object>>?>()))
            .ReturnsAsync(streetcodeArts);
    }

    private void SetupMapper()
    {
        _mockMapper.Setup(mapper => mapper
            .Map<IEnumerable<StreetcodeArtDTO>>(It.IsAny<IEnumerable<StreetcodeArt>?>()))
            .Returns(new List<StreetcodeArtDTO>());
    }

    private void SetupBlobService()
    {
        _mockBlobService.Setup(blobService => blobService
            .FindFileInStorageAsBase64(It.IsAny<string>()))
            .Returns(It.IsAny<string>());
    }

    private void SetupStringLocalizer(int id, string errorMsg)
    {
        _mockStringLocalizerCannotFind.Setup(stringLocalizer => stringLocalizer["CannotFindAnyArtWithCorrespondingStreetcodeId", id])
            .Returns(new LocalizedString("CannotFindAnyArtWithCorrespondingStreetcodeId", errorMsg));
    }
}