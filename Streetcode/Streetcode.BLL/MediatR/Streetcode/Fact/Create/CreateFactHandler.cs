using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Fact.Create;

public class CreateFactHandler : IRequestHandler<CreateFactCommand, Result<Unit>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IStringLocalizer<CannotConvertNullSharedResource> _stringLocalizerCannot;
    private readonly IStringLocalizer<FailedToCreateSharedResource> _stringLocalizerFailed;

    public CreateFactHandler(
        IRepositoryWrapper repositoryWrapper,
        IMapper mapper,
        IStringLocalizer<FailedToCreateSharedResource> stringLocalizerFailed,
        IStringLocalizer<CannotConvertNullSharedResource> stringLocalizerCannot)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _stringLocalizerFailed = stringLocalizerFailed;
        _stringLocalizerCannot = stringLocalizerCannot;
    }

    public async Task<Result<Unit>> Handle(CreateFactCommand request, CancellationToken cancellationToken)
    {
        var fact = _mapper.Map<DAL.Entities.Streetcode.TextContent.Fact>(request.Fact);

        if (fact is null)
        {
            return Result.Fail(new Error(_stringLocalizerCannot["CannotConvertNullToFact"].Value));
        }

        _repositoryWrapper.FactRepository.Create(fact);

        var resultIsSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;
        return resultIsSuccess ? Result.Ok(Unit.Value) : Result.Fail(new Error(_stringLocalizerFailed["FailedToCreateFact"].Value));
    }
}