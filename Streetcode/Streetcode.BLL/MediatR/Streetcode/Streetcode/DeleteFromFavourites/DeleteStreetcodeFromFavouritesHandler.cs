using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.DeleteFromFavourites
{
    public class DeleteStreetcodeFromFavouritesHandler : IRequestHandler<DeleteStreetcodeFromFavouritesCommand, Result<Unit>>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;
        private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;
        private readonly IStringLocalizer<FailedToDeleteSharedResource> _stringLocalizerFailedToDelete;

        public DeleteStreetcodeFromFavouritesHandler(IRepositoryWrapper repositoryWrapper, ILoggerService logger, IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind, IStringLocalizer<FailedToDeleteSharedResource> stringLocalizerFailedToDelete)
        {
            _repositoryWrapper = repositoryWrapper;
            _logger = logger;
            _stringLocalizerCannotFind = stringLocalizerCannotFind;
            _stringLocalizerFailedToDelete = stringLocalizerFailedToDelete;
        }

        public async Task<Result<Unit>> Handle(DeleteStreetcodeFromFavouritesCommand request, CancellationToken cancellationToken)
        {
            var streetcode = await _repositoryWrapper.StreetcodeRepository
            .GetFirstOrDefaultAsync(
            predicate: s => s.Id == request.streetcodeId);

            if (streetcode is null)
            {
                string errorMsg = _stringLocalizerCannotFind["CannotFindAnyStreetcodeWithCorrespondingId", request.streetcodeId].Value;
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            var favourite = await _repositoryWrapper.FavouritesRepository.GetFirstOrDefaultAsync(
                 f => f.UserId == request.userId && f.StreetcodeId == request.streetcodeId);

            if(favourite is null)
            {
                string errorMsg = "Streetcode is not in favourites";
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            _repositoryWrapper.FavouritesRepository.Delete(favourite);
            var resultIsSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;

            if (resultIsSuccess)
            {
                return Result.Ok(Unit.Value);
            }
            else
            {
                string errorMsg = _stringLocalizerFailedToDelete["FailedToDeleteStreetcode"].Value;
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }
        }
    }
}
