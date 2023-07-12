using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Media.Image.GetByStreetcodeId;

public class GetImageByStreetcodeIdHandler : IRequestHandler<GetImageByStreetcodeIdQuery, Result<IEnumerable<ImageDTO>>>
{
    private readonly IBlobService _blobService;
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService _logger;

    public GetImageByStreetcodeIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, IBlobService blobService, ILoggerService logger)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _blobService = blobService;
        _logger = logger;
    }

    public async Task<Result<IEnumerable<ImageDTO>>> Handle(GetImageByStreetcodeIdQuery request, CancellationToken cancellationToken)
    {
        var images = (await _repositoryWrapper.ImageRepository
            .GetAllAsync(
            f => f.Streetcodes.Any(s => s.Id == request.StreetcodeId),
            include: q => q.Include(img => img.ImageDetails))).OrderBy(img => img.ImageDetails?.Alt);

        if (images is null || images.Count() == 0)
        {
            string errorMsg = $"Cannot find an image with the corresponding streetcode id: {request.StreetcodeId}";
            _logger.LogError(request, errorMsg);
            return Result.Fail(new Error(errorMsg));
        }

        var imageDtos = _mapper.Map<IEnumerable<ImageDTO>>(images);

        foreach (var image in imageDtos)
        {
            image.Base64 = _blobService.FindFileInStorageAsBase64(image.BlobName);
        }

        return Result.Ok(imageDtos);
    }
}