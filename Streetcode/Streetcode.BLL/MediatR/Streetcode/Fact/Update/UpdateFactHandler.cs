using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Fact.Update;

public class UpdateFactHandler : IRequestHandler<UpdateFactCommand, Result<Unit>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IStringLocalizer<CannotConvertNullSharedResource> _stringLocalizerCannotConvert;
    private readonly IStringLocalizer<FailedToUpdateSharedResource> _stringLocalizerFailedToUpdate;

    public UpdateFactHandler(
        IRepositoryWrapper repositoryWrapper,
        IMapper mapper,
        IStringLocalizer<FailedToUpdateSharedResource> stringLocalizerFailedToUpdate,
        IStringLocalizer<CannotConvertNullSharedResource> stringLocalizerCannotConvert)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _stringLocalizerFailedToUpdate = stringLocalizerFailedToUpdate;
        _stringLocalizerCannotConvert = stringLocalizerCannotConvert;
    }

    public async Task<Result<Unit>> Handle(UpdateFactCommand request, CancellationToken cancellationToken)
    {
        var fact = _mapper.Map<DAL.Entities.Streetcode.TextContent.Fact>(request.Fact);

        if (fact is null)
        {
            return Result.Fail(new Error(_stringLocalizerCannotConvert["CannotConvertNullToFact"].Value));
        }

        _repositoryWrapper.FactRepository.Update(fact);

        var resultIsSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;
        return resultIsSuccess ? Result.Ok(Unit.Value) : Result.Fail(new Error(_stringLocalizerFailedToUpdate["FailedToUpdateFact"].Value));
    }
}