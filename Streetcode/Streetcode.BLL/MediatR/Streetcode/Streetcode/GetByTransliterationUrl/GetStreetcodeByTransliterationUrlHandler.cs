﻿using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.AdditionalContent.Tag;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.GetByTransliterationUrl;

public class GetStreetcodeByTransliterationUrlHandler : IRequestHandler<GetStreetcodeByTransliterationUrlQuery, Result<StreetcodeDTO>>
{
    private readonly IRepositoryWrapper _repository;
    private readonly IMapper _mapper;
    private readonly ILoggerService _logger;
    private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;

    public GetStreetcodeByTransliterationUrlHandler(
        IRepositoryWrapper repository,
        IMapper mapper,
        ILoggerService logger,
        IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
        _stringLocalizerCannotFind = stringLocalizerCannotFind;
    }

    public async Task<Result<StreetcodeDTO>> Handle(GetStreetcodeByTransliterationUrlQuery request, CancellationToken cancellationToken)
    {
        var streetcode = await _repository.StreetcodeRepository
            .GetFirstOrDefaultAsync(
                predicate: st => st.TransliterationUrl == request.Url);

        if (streetcode == null)
        {
            var errorMsg = _stringLocalizerCannotFind["CannotFindStreetcodeByTransliterationUrl", request.Url].Value;
            _logger.LogError(request, errorMsg);
            return new Error(errorMsg);
        }

        var tagIndexed = await _repository.StreetcodeTagIndexRepository
            .GetAllAsync(
                t => t.StreetcodeId == streetcode.Id,
                include: q => q.Include(ti => ti.Tag!));

        var streetcodeDto = _mapper.Map<StreetcodeDTO>(streetcode);
        streetcodeDto.Tags = _mapper.Map<List<StreetcodeTagDTO>>(tagIndexed);

        if(streetcodeDto.Tags is not null)
        {
            streetcodeDto.Tags = streetcodeDto.Tags.OrderBy(tag => tag.Index);
        }

        return Result.Ok(streetcodeDto);
    }
}