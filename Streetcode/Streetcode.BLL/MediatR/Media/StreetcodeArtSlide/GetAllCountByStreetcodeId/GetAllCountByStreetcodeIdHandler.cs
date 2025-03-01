using System.Linq.Expressions;
using FluentResults;
using MediatR;
using Streetcode.BLL.Services.EntityAccessManager;
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

        public Task<Result<int>> Handle(GetAllCountByStreetcodeIdQuerry request, CancellationToken cancellationToken)
        {
            Expression<Func<DAL.Entities.Streetcode.StreetcodeArtSlide, bool>>? basePredicate = sArtSlide => sArtSlide.StreetcodeId == request.StreetcodeId;
            var predicate = basePredicate.ExtendWithAccessPredicate(new StreetcodeAccessManager(), request.UserRole, sArtSlide => sArtSlide.Streetcode);

            var slidesCount = _repositoryWrapper.StreetcodeArtSlideRepository
                .FindAll(predicate: predicate)
                .Count();

            return Task.FromResult(Result.Ok(slidesCount));
        }
    }
}
