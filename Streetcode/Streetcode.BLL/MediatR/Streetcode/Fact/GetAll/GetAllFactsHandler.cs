﻿using System.Linq.Expressions;
using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Streetcode.TextContent.Fact;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Services.EntityAccessManager;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Fact.GetAll;

public class GetAllFactsHandler : IRequestHandler<GetAllFactsQuery, Result<IEnumerable<FactDto>>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService _logger;
    private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizeCannotFind;

    public GetAllFactsHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger, IStringLocalizer<CannotFindSharedResource> stringLocalizeCannotFind)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _logger = logger;
        _stringLocalizeCannotFind = stringLocalizeCannotFind;
    }

    public async Task<Result<IEnumerable<FactDto>>> Handle(GetAllFactsQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<DAL.Entities.Streetcode.TextContent.Fact, bool>>? basePredicate = null;
        var predicate = basePredicate.ExtendWithAccessPredicate(new StreetcodeAccessManager(), request.UserRole, f => f.Streetcode);

        var facts = await _repositoryWrapper.FactRepository.GetAllAsync(predicate: predicate);

        if (facts is null)
        {
            string errorMsg = _stringLocalizeCannotFind["CannotFindAnyFact"].Value;
            _logger.LogError(request, errorMsg);
            return Result.Fail(new Error(errorMsg));
        }

        return Result.Ok(_mapper.Map<IEnumerable<FactDto>>(facts));
    }
}