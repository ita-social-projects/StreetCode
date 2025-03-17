using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Streetcode.TextContent.Term;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;
using TermEntity = Streetcode.DAL.Entities.Streetcode.TextContent.Term;

namespace Streetcode.BLL.MediatR.Streetcode.Term.Create;

public class CreateTermHandler : IRequestHandler<CreateTermCommand, Result<TermDto>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repository;
    private readonly ILoggerService _logger;
    private readonly IStringLocalizer<CannotConvertNullSharedResource> _stringLocalizerCannotConvert;
    private readonly IStringLocalizer<FailedToCreateSharedResource> _stringLocalizerFailedToCreate;

    public CreateTermHandler(
        IMapper mapper,
        IRepositoryWrapper repository,
        ILoggerService logger,
        IStringLocalizer<FailedToCreateSharedResource> stringLocalizerFailedToCreate,
        IStringLocalizer<CannotConvertNullSharedResource> stringLocalizerCannotConvert)
    {
        _mapper = mapper;
        _repository = repository;
        _logger = logger;
        _stringLocalizerFailedToCreate = stringLocalizerFailedToCreate;
        _stringLocalizerCannotConvert = stringLocalizerCannotConvert;
    }

    public async Task<Result<TermDto>> Handle(CreateTermCommand request, CancellationToken cancellationToken)
    {
        var term = _mapper.Map<TermEntity>(request.Term);

        if (term is null)
        {
            var errorMessage = _stringLocalizerCannotConvert["CannotConvertNullToTerm"].Value;
            _logger.LogError(request, errorMessage);
            return Result.Fail(new Error(errorMessage));
        }

        var createdTerm = await _repository.TermRepository.CreateAsync(term);
        var resultIsSuccess = await _repository.SaveChangesAsync() > 0;

        if (!resultIsSuccess)
        {
            var errorMessage = _stringLocalizerFailedToCreate["FailedToCreateTerm"].Value;
            _logger.LogError(request, errorMessage);
            return Result.Fail(new Error(errorMessage));
        }

        var createdTermDto = _mapper.Map<TermDto>(createdTerm);

        if (createdTermDto is null)
        {
            var errorMessage = _stringLocalizerFailedToCreate["FailedToMapCreatedTerm"].Value;
            _logger.LogError(request, errorMessage);
            return Result.Fail(new Error(errorMessage));
        }

        return Result.Ok(createdTermDto);
    }
}
