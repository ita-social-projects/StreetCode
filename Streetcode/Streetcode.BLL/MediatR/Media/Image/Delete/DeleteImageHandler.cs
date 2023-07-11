using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Media.Audio.Delete;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Media.Image.Delete;

public class DeleteImageHandler : IRequestHandler<DeleteImageCommand, Result<Unit>>
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IBlobService _blobService;
    private readonly ILoggerService _logger;

    public DeleteImageHandler(IRepositoryWrapper repositoryWrapper, IBlobService blobService, ILoggerService logger)
    {
        _repositoryWrapper = repositoryWrapper;
        _blobService = blobService;
        _logger = logger;
    }

    public async Task<Result<Unit>> Handle(DeleteImageCommand request, CancellationToken cancellationToken)
    {
        var image = await _repositoryWrapper.ImageRepository
            .GetFirstOrDefaultAsync(
            predicate: i => i.Id == request.Id,
            include: s => s.Include(i => i.Streetcodes));

        if (image is null)
        {
            string errorMsg = $"Cannot find an image with corresponding categoryId: {request.Id}";
            _logger.LogError(request, errorMsg);
            return Result.Fail(new Error(errorMsg));
        }

        _repositoryWrapper.ImageRepository.Delete(image);

        var resultIsSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;

        if (resultIsSuccess)
        {
            _blobService.DeleteFileInStorage(image.BlobName);
        }

        if(resultIsSuccess)
        {
            return Result.Ok(Unit.Value);
        }
        else
        {
            const string errorMsg = $"Failed to delete an image";
            _logger.LogError(request, errorMsg);
            return Result.Fail(new Error(errorMsg));
        }
    }
}