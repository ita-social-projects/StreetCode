﻿using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.AdditionalContent.Subtitles;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Streetcode.TextContent.Fact;
using Streetcode.BLL.Interfaces.Logging;
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
        var fact = await _repositoryWrapper.FactRepository
            .GetAllAsync(f => f.StreetcodeId == request.StreetcodeId);

        if (fact is null)
        {
            string errorMsg = _stringLocalizerCannotFind["CannotFindAnyFactByTheStreetcodeId", request.StreetcodeId].Value;
            _logger.LogError(request, errorMsg);
            return Result.Fail(new Error(errorMsg));
        }

        return Result.Ok(_mapper.Map<IEnumerable<FactDto>>(fact));
    }
}