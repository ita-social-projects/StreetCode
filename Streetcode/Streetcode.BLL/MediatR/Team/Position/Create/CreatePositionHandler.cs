﻿using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Team;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Team.Create;

public class CreatePositionHandler : IRequestHandler<CreatePositionQuery, Result<PositionDTO>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repository;
    private readonly ILoggerService _logger;
    private readonly IStringLocalizer<FailedToValidateSharedResource> _failedToValidateSharedResource;
    private readonly IStringLocalizer<FieldNamesSharedResource> _fieldNamesSharedResource;

    public CreatePositionHandler(
        IMapper mapper,
        IRepositoryWrapper repository,
        ILoggerService logger,
        IStringLocalizer<FailedToValidateSharedResource> failedToValidateSharedResource,
        IStringLocalizer<FieldNamesSharedResource> fieldNamesSharedResource)
    {
        _mapper = mapper;
        _repository = repository;
        _logger = logger;
        _failedToValidateSharedResource = failedToValidateSharedResource;
        _fieldNamesSharedResource = fieldNamesSharedResource;
    }

    public async Task<Result<PositionDTO>> Handle(CreatePositionQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var context = _mapper.Map<Positions>(request.position);
            var checkIfContextExists = await _repository.PositionRepository.GetFirstOrDefaultAsync(j => j.Position == request.position.Position);

            if (checkIfContextExists is not null)
            {
                string exceptionMessege = _failedToValidateSharedResource["MustBeUnique", _fieldNamesSharedResource["Position"]];
                _logger.LogError(request, exceptionMessege);
                return Result.Fail(exceptionMessege);
            }

            var createdContext = await _repository.PositionRepository.CreateAsync(context);
            await _repository.SaveChangesAsync();
            return Result.Ok(_mapper.Map<PositionDTO>(createdContext));
        }
        catch (Exception ex)
        {
            _logger.LogError(request, ex.Message);
            return Result.Fail(ex.Message);
        }
    }
}