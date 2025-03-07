using System.Linq.Expressions;
using FluentResults;
using MediatR;
using Streetcode.BLL.Services.EntityAccessManager;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.WithUrlExist;

public class StreetcodeWithUrlExistHandler : IRequestHandler<StreetcodeWithUrlExistQuery, Result<bool>>
{
    private readonly IRepositoryWrapper _repository;

    public StreetcodeWithUrlExistHandler(IRepositoryWrapper repository)
    {
        _repository = repository;
    }

    public async Task<Result<bool>> Handle(StreetcodeWithUrlExistQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<StreetcodeContent, bool>>? basePredicate = st => st.TransliterationUrl == request.Url;
        var predicate = basePredicate.ExtendWithAccessPredicate(new StreetcodeAccessManager(), request.UserRole);

        var streetcodes = await _repository.StreetcodeRepository.GetFirstOrDefaultAsync(predicate: predicate);
        return Result.Ok(streetcodes != null);
    }
}