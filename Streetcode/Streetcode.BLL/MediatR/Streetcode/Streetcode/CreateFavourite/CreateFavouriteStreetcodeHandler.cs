using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.UpdateToFavourites
{
    public class CreateFavouriteStreetcodeHandler : IRequestHandler<CreateFavouriteStreetcodeCommand, Result<Unit>>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;
        private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;

        public CreateFavouriteStreetcodeHandler(IRepositoryWrapper repositoryWrapper, ILoggerService logger, IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind)
        {
            _repositoryWrapper = repositoryWrapper;
            _logger = logger;
            _stringLocalizerCannotFind = stringLocalizerCannotFind;
        }

        public async Task<Result<Unit>> Handle(CreateFavouriteStreetcodeCommand request, CancellationToken cancellationToken)
        {
            var streetcode = await _repositoryWrapper.StreetcodeRepository
                .GetFirstOrDefaultAsync(s => s.Id == request.streetcodeId);
            if (streetcode is null)
            {
                string errorMsg = _stringLocalizerCannotFind["CannotFindAnyStreetcodeWithCorrespondingId", request.streetcodeId].Value;
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            if (await _repositoryWrapper.FavouritesRepository.GetFirstOrDefaultAsync(
                 f => f.UserId == request.userId && f.StreetcodeId == request.streetcodeId) is not null)
            {
                string errorMsg = "Streetcode is already in favourites";
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            var favourite = new DAL.Entities.Users.Favourites.Favourites
            {
                StreetcodeId = request.streetcodeId,
                UserId = request.userId
            };

            await _repositoryWrapper.FavouritesRepository.CreateAsync(favourite);

            var resultIsSuccessChangeStatus = await _repositoryWrapper.SaveChangesAsync() > 0;

            if (resultIsSuccessChangeStatus)
            {
                return Result.Ok(Unit.Value);
            }

            string errorMessage = "Cannot save updated streetcode";
            _logger.LogError(request, errorMessage);
            return Result.Fail(new Error(errorMessage));
        }
    }
}
