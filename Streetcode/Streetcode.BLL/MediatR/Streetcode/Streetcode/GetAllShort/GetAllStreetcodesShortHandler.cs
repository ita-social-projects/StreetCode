﻿using System.Linq.Expressions;
using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Services.EntityAccessManager;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.GetAllShort;

public class GetAllStreetcodesShortHandler : IRequestHandler<GetAllStreetcodesShortQuery,
    Result<IEnumerable<StreetcodeShortDTO>>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService _logger;
    private readonly IStringLocalizer<NoSharedResource> _stringLocalizerNo;

    public GetAllStreetcodesShortHandler(
        IRepositoryWrapper repositoryWrapper,
        IMapper mapper,
        ILoggerService logger,
        IStringLocalizer<NoSharedResource> stringLocalizerNo)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _logger = logger;
        _stringLocalizerNo = stringLocalizerNo;
    }

    public async Task<Result<IEnumerable<StreetcodeShortDTO>>> Handle(GetAllStreetcodesShortQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<StreetcodeContent, bool>>? basePredicate = null;
            var predicate = basePredicate.ExtendWithAccessPredicate(new StreetcodeAccessManager(), request.UserRole);

            var streetcodes = await _repositoryWrapper.StreetcodeRepository.GetAllAsync(predicate: predicate);

            if (streetcodes.Any())
            {
                return Result.Ok(_mapper.Map<IEnumerable<StreetcodeShortDTO>>(streetcodes));
            }

            var errorMsg = _stringLocalizerNo["NoStreetcodesExistNow"].Value;
            _logger.LogError(request, errorMsg);
            return Result.Fail(errorMsg);
    }
}