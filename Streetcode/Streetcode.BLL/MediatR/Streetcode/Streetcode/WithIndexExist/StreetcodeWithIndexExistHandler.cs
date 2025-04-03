using System.Linq.Expressions;
using FluentResults;
using MediatR;
using Streetcode.BLL.Services.EntityAccessManager;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.WithIndexExist;

public class StreetcodeWithIndexExistHandler : IRequestHandler<StreetcodeWithIndexExistQuery, Result<bool>>
{
    private readonly IRepositoryWrapper _repositoryWrapper;

    public StreetcodeWithIndexExistHandler(IRepositoryWrapper repositoryWrapper)
    {
        _repositoryWrapper = repositoryWrapper;
    }

    public async Task<Result<bool>> Handle(StreetcodeWithIndexExistQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<StreetcodeContent, bool>>? basePredicate = s => s.Index == request.Index;
        var predicate = basePredicate.ExtendWithAccessPredicate(new StreetcodeAccessManager(), request.UserRole);

        var streetcode = await _repositoryWrapper.StreetcodeRepository.GetFirstOrDefaultAsync(predicate: predicate);

        return Result.Ok(streetcode != null);
    }
}