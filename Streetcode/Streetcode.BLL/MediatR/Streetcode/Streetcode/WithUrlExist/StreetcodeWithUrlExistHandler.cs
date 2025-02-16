using FluentResults;
using MediatR;
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
        var streetcodes = await _repository.StreetcodeRepository.GetFirstOrDefaultAsync(predicate: st => st.TransliterationUrl == request.Url);
        return Result.Ok(streetcodes != null);
    }
}