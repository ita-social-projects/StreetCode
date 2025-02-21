using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.BLL.Util.Helpers;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.DeleteFromFavourites
{
    public class DeleteStreetcodeFromFavouritesHandler : IRequestHandler<DeleteStreetcodeFromFavouritesCommand, Result<Unit>>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;
        private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;
        private readonly IStringLocalizer<FailedToDeleteSharedResource> _stringLocalizerFailedToDelete;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DeleteStreetcodeFromFavouritesHandler(IRepositoryWrapper repositoryWrapper, ILoggerService logger, IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind, IStringLocalizer<FailedToDeleteSharedResource> stringLocalizerFailedToDelete, IHttpContextAccessor httpContextAccessor)
        {
            _repositoryWrapper = repositoryWrapper;
            _logger = logger;
            _stringLocalizerCannotFind = stringLocalizerCannotFind;
            _stringLocalizerFailedToDelete = stringLocalizerFailedToDelete;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result<Unit>> Handle(DeleteStreetcodeFromFavouritesCommand request, CancellationToken cancellationToken)
        {
            var userId = HttpContextHelper.GetCurrentUserId(_httpContextAccessor)!;

            var favourite = await _repositoryWrapper.FavouritesRepository.GetFirstOrDefaultAsync(
                 f => f.UserId == userId && f.StreetcodeId == request.StreetcodeId);

            if (favourite is null)
            {
                string errorMsg = _stringLocalizerCannotFind["CannotFindStreetcodeInFavourites"].Value;
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
