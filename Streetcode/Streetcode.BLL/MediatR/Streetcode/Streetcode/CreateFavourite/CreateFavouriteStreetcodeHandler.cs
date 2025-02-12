using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Streetcode.Favourites;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.CreateFavourite
{
    public class CreateFavouriteStreetcodeHandler : IRequestHandler<CreateFavouriteStreetcodeCommand, Result<Unit>>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;
        private readonly IStringLocalizer<AlreadyExistSharedResource> _stringLocalizerAlreadyExists;
        private readonly IStringLocalizer<CannotSaveSharedResource> _stringLocalizerCannotSave;

        public CreateFavouriteStreetcodeHandler(IRepositoryWrapper repositoryWrapper, ILoggerService logger, IStringLocalizer<AlreadyExistSharedResource> stringLocalizerAlreadyExists, IStringLocalizer<CannotSaveSharedResource> stringLocalizerCannotSave)
        {
            _repositoryWrapper = repositoryWrapper;
            _logger = logger;
            _stringLocalizerAlreadyExists = stringLocalizerAlreadyExists;
            _stringLocalizerCannotSave = stringLocalizerCannotSave;
        }

        public async Task<Result<Unit>> Handle(CreateFavouriteStreetcodeCommand request, CancellationToken cancellationToken)
        {
            if (await _repositoryWrapper.FavouritesRepository.GetFirstOrDefaultAsync(
                 f => f.UserId == request.userId && f.StreetcodeId == request.streetcodeId) is not null)
            {
                string errorMsg = _stringLocalizerAlreadyExists["FavouriteAlreadyExists"].Value;
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            var favourite = new Favourite
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

            string errorMessage = _stringLocalizerCannotSave["CannotSaveTheData"].Value;
            _logger.LogError(request, errorMessage);
            return Result.Fail(new Error(errorMessage));
        }
    }
}
