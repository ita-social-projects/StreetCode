﻿using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Streetcode.TextContent.Text;
using Streetcode.BLL.DTO.Transactions;
using Streetcode.BLL.Interfaces.Cache;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Interfaces.Text;
using Streetcode.BLL.MediatR.ResultVariations;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Text.GetByStreetcodeId;

public class GetTextByStreetcodeIdHandler : IRequestHandler<GetTextByStreetcodeIdQuery, Result<TextDTO?>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ITextService _textService;
    private readonly ILoggerService _logger;
    private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;
    private readonly ICacheService _cacheService;

    public GetTextByStreetcodeIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ITextService textService, ILoggerService logger, IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind, ICacheService cacheService)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _textService = textService;
        _logger = logger;
        _stringLocalizerCannotFind = stringLocalizerCannotFind;
        _cacheService = cacheService;
    }

    public async Task<Result<TextDTO?>> Handle(GetTextByStreetcodeIdQuery request, CancellationToken cancellationToken)
    {
        string cacheKey = $"TextCache_{request.StreetcodeId}";

        return await _cacheService.GetOrSetAsync(
                cacheKey,
                async () =>
                {
                    var text = await _repositoryWrapper.TextRepository
            .GetFirstOrDefaultAsync(text => text.StreetcodeId == request.StreetcodeId);

                    if (text is null)
                    {
                        if (await _repositoryWrapper.StreetcodeRepository
                             .GetFirstOrDefaultAsync(s => s.Id == request.StreetcodeId) == null)
                        {
                            string errorMsg = _stringLocalizerCannotFind["CannotFindTransactionLinkByStreetcodeIdBecause", request.StreetcodeId].Value;
                            _logger.LogError(request, errorMsg);
                            return Result.Fail<TextDTO?>(new Error(errorMsg));
                        }
                    }

                    NullResult<TextDTO?> result = new NullResult<TextDTO?>();
                    if (text != null)
                    {
                        result.WithValue(_mapper.Map<TextDTO?>(text));
                    }

                    return result;
                },
                TimeSpan.FromMinutes(10));
    }
}