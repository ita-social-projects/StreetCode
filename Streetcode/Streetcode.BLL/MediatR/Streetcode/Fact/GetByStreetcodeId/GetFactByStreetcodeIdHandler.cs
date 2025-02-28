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

namespace Streetcode.BLL.MediatR.Streetcode.Fact.GetByStreetcodeId;

public class GetFactByStreetcodeIdHandler : IRequestHandler<GetFactByStreetcodeIdQuery, Result<IEnumerable<FactDto>>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService _logger;
    private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;

    public GetFactByStreetcodeIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger, IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _logger = logger;
        _stringLocalizerCannotFind = stringLocalizerCannotFind;
    }

    public async Task<Result<IEnumerable<FactDto>>> Handle(GetFactByStreetcodeIdQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<DAL.Entities.Streetcode.TextContent.Fact, bool>>? basePredicate = f => f.StreetcodeId == request.StreetcodeId;

        var predicate = basePredicate.ExtendWithAccessPredicate(new StreetcodeAccessManager(), request.UserRole, f => f.Streetcode);

        var facts = await _repositoryWrapper.FactRepository
            .GetAllAsync(predicate: predicate);

        if (!facts.Any())
        {
            string message = "Returning empty enumerable of facts";
            _logger.LogInformation(message);
            return Result.Ok(Enumerable.Empty<FactDto>());
        }

        return Result.Ok(_mapper.Map<IEnumerable<FactDto>>(facts.OrderBy(f => f.Index)));
    }
}
