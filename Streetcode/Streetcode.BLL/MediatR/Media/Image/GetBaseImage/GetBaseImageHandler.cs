using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Media.Image.GetBaseImage;

public class GetBaseImageHandler : IRequestHandler<GetBaseImageQuery, Result<MemoryStream>>
{
    private readonly IBlobService _blobStorage;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;

    public GetBaseImageHandler(IBlobService blobService, IRepositoryWrapper repositoryWrapper, IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind)
    {
        _blobStorage = blobService;
        _repositoryWrapper = repositoryWrapper;
        _stringLocalizerCannotFind = stringLocalizerCannotFind;
    }

    public async Task<Result<MemoryStream>> Handle(GetBaseImageQuery request, CancellationToken cancellationToken)
    {
        var image = await _repositoryWrapper.ImageRepository.GetFirstOrDefaultAsync(a => a.Id == request.Id);

        if (image is null)
        {
            return Result.Fail(new Error(_stringLocalizerCannotFind["CannotFindAnAudioWithCorrespondingId", request.Id]));
        }

        var imageFile = _blobStorage.FindFileInStorageAsMemoryStream(image.BlobName);

        return imageFile;
    }
}
