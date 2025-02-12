using FluentResults;
using MediatR;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.GetFavouriteStatus
{
    public class GetFavouriteStatusHandler : IRequestHandler<GetFavouriteStatusQuery,
        Result<bool>>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;

        public GetFavouriteStatusHandler(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<Result<bool>> Handle(GetFavouriteStatusQuery request, CancellationToken cancellationToken)
        {
            var favourite = await _repositoryWrapper.StreetcodeRepository.GetFirstOrDefaultAsync(
                fv => fv.UserFavourites.Any(u => u.Id == request.userId) && fv.Id == request.streetcodeId);
            return Result.Ok(favourite != null);
        }
    }
}
