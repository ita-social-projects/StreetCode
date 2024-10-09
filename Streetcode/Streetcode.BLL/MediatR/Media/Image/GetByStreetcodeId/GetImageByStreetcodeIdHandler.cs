using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.Interfaces.Cache;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Media.Image.GetByStreetcodeId;

public class GetImageByStreetcodeIdHandler : IRequestHandler<GetImageByStreetcodeIdQuery, Result<IEnumerable<ImageDTO>>>
{
    private readonly IBlobService _blobService;
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService _logger;
    private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;
    private readonly ICacheService _cacheService;

    public GetImageByStreetcodeIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, IBlobService blobService, ILoggerService logger, IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind, ICacheService cacheService)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _blobService = blobService;
        _logger = logger;
        _stringLocalizerCannotFind = stringLocalizerCannotFind;
        _cacheService = cacheService;
    }

    public async Task<Result<IEnumerable<ImageDTO>>> Handle(GetImageByStreetcodeIdQuery request, CancellationToken cancellationToken)
    {
        string cacheKey = $"ImageCache_{request.StreetcodeId}";
        _logger.LogInformation(cacheKey + "cacheKey was created");
        return await _cacheService.GetOrSetAsync(
            cacheKey,
            async () =>
            {
                _logger.LogInformation(cacheKey + "async function start");
                var images = (await _repositoryWrapper.ImageRepository
                    .GetAllAsync(
                    f => f.Streetcodes.Any(s => s.Id == request.StreetcodeId),
                    include: q => q.Include(img => img.ImageDetails!))).OrderBy(img => img.ImageDetails?.Alt);

                if (images is null || request.StreetcodeId < 1)
                {
                    string errorMsg = _stringLocalizerCannotFind["CannotFindAnImageWithTheCorrespondingStreetcodeId", request.StreetcodeId].Value;
                    _logger.LogError(request, errorMsg);
                    return Result.Fail(new Error(errorMsg));
                }

                var imageDtos = _mapper.Map<IEnumerable<ImageDTO>>(images);

                foreach (var image in imageDtos)
                {
                    image.Base64 = _blobService.FindFileInStorageAsBase64(image.BlobName);
                }

                _logger.LogInformation(cacheKey + "async function finish");
                return Result.Ok(imageDtos);
            },
            TimeSpan.FromMinutes(10));
    }
}