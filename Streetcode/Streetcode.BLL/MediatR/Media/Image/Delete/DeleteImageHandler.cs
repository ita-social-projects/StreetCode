using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.MediatR.Media.Audio.Delete;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Media.Image.Delete;

public class DeleteImageHandler : IRequestHandler<DeleteImageCommand, Result<Unit>>
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IBlobService _blobService;
    private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;
    private readonly IStringLocalizer<FailedToDeleteSharedResource> _stringLocalizerFailedToDelete;

    public DeleteImageHandler(
        IRepositoryWrapper repositoryWrapper,
        IBlobService blobService,
        IStringLocalizer<FailedToDeleteSharedResource> stringLocalizerFailedToDelete,
        IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind)
    {
        _repositoryWrapper = repositoryWrapper;
        _blobService = blobService;
        _stringLocalizerCannotFind = stringLocalizerCannotFind;
        _stringLocalizerFailedToDelete = stringLocalizerFailedToDelete;
    }

    public async Task<Result<Unit>> Handle(DeleteImageCommand request, CancellationToken cancellationToken)
    {
        var image = await _repositoryWrapper.ImageRepository
            .GetFirstOrDefaultAsync(
            predicate: i => i.Id == request.Id,
            include: s => s.Include(i => i.Streetcodes));

        if (image is null)
        {
            return Result.Fail(new Error(_stringLocalizerCannotFind["CannotFindAnyImage", request.Id].Value));
        }

        _repositoryWrapper.ImageRepository.Delete(image);

        var resultIsSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;

        if (resultIsSuccess)
        {
            _blobService.DeleteFileInStorage(image.BlobName);
        }

        return resultIsSuccess ? Result.Ok(Unit.Value) : Result.Fail(new Error(_stringLocalizerFailedToDelete["FailedToDeleteImage"].Value));
    }
}