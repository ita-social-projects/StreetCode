using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Http;
using Streetcode.BLL.Util.Helpers;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.GetFavouriteStatus
{
    public class GetFavouriteStatusHandler : IRequestHandler<GetFavouriteStatusQuery,
        Result<bool>>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetFavouriteStatusHandler(IRepositoryWrapper repositoryWrapper, IHttpContextAccessor httpContextAccessor)
        {
            _repositoryWrapper = repositoryWrapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result<bool>> Handle(GetFavouriteStatusQuery request, CancellationToken cancellationToken)
        {
            var userId = HttpContextHelper.GetCurrentUserId(_httpContextAccessor);

            var favourite = await _repositoryWrapper.StreetcodeRepository.GetFirstOrDefaultAsync(
                fv => fv.UserFavourites.Any(u => u.Id == userId) && fv.Id == request.StreetcodeId);
            return Result.Ok(favourite != null);
        }
    }
}
