using FluentResults;
using MediatR;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Media.Art.StreetcodeArtSlide.GetAllCountByStreetcodeId
{
    public class GetAllCountByStreetcodeIdHandler : IRequestHandler<GetAllCountByStreetcodeIdQuerry, Result<int>>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;

        public GetAllCountByStreetcodeIdHandler(
            IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<Result<int>> Handle(GetAllCountByStreetcodeIdQuerry request, CancellationToken cancellationToken)
        {
            var slidesCount = _repositoryWrapper.StreetcodeArtSlideRepository
                .FindAll(
                    predicate: sArtSlide => sArtSlide.StreetcodeId == request.StreetcodeId)
                .Count();

            return Result.Ok(slidesCount);
        }
    }
}
