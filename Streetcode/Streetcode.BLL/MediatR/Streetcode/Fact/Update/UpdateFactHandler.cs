using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.Interfaces.Logging;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Fact.Update;

public class UpdateFactHandler : IRequestHandler<UpdateFactCommand, Result<Unit>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService _logger;
    private readonly IStringLocalizer<CannotConvertNullSharedResource> _stringLocalizerCannotConvert;
    private readonly IStringLocalizer<FailedToUpdateSharedResource> _stringLocalizerFailedToUpdate;

    public UpdateFactHandler(
        IRepositoryWrapper repositoryWrapper,
        IMapper mapper,
        ILoggerService logger,
        IStringLocalizer<FailedToUpdateSharedResource> stringLocalizerFailedToUpdate,
        IStringLocalizer<CannotConvertNullSharedResource> stringLocalizerCannotConvert)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _logger = logger;
        _stringLocalizerFailedToUpdate = stringLocalizerFailedToUpdate;
        _stringLocalizerCannotConvert = stringLocalizerCannotConvert;
    }

    public async Task<Result<Unit>> Handle(UpdateFactCommand request, CancellationToken cancellationToken)
    {
        var fact = _mapper.Map<DAL.Entities.Streetcode.TextContent.Fact>(request.Fact);

        if (fact is null)
        {
            string errorMsg = _stringLocalizerCannotConvert["CannotConvertNullToFact"].Value;
            _logger.LogError(request, errorMsg);
            return Result.Fail(new Error(errorMsg));
        }

        _repositoryWrapper.FactRepository.Update(fact);

        var resultIsSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;
        if(resultIsSuccess)
        {
            return Result.Ok(Unit.Value);
        }
        else
        {
            string errorMsg = _stringLocalizerFailedToUpdate["FailedToUpdateFact"].Value;
            _logger.LogError(request, errorMsg);
            return Result.Fail(new Error(errorMsg));
        }
    }
}