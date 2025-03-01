using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Streetcode.TextContent.Text;
using Streetcode.BLL.Interfaces.Cache;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.ResultVariations;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Text.GetByStreetcodeId;

public class GetTextByStreetcodeIdHandler : IRequestHandler<GetTextByStreetcodeIdQuery, Result<TextDTO>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService _logger;
    private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;
    private readonly ICacheService _cacheService;

    public GetTextByStreetcodeIdHandler(
        IRepositoryWrapper repositoryWrapper,
        IMapper mapper,
        ILoggerService logger,
        IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind,
        ICacheService cacheService)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _logger = logger;
        _stringLocalizerCannotFind = stringLocalizerCannotFind;
        _cacheService = cacheService;
    }

    public async Task<Result<TextDTO>> Handle(GetTextByStreetcodeIdQuery request, CancellationToken cancellationToken)
    {
        string cacheKey = $"TextCache_{request.StreetcodeId}";

        return await _cacheService.GetOrSetAsync(
                cacheKey,
                async () =>
                {
                    var text = await _repositoryWrapper.TextRepository
                        .GetFirstOrDefaultAsync(x => x.StreetcodeId == request.StreetcodeId);

                    if (text is null && await _repositoryWrapper.StreetcodeRepository
                            .GetFirstOrDefaultAsync(x => x.Id == request.StreetcodeId) is null)
                    {
                        var errorMessage = _stringLocalizerCannotFind["CannotFindTransactionLinkByStreetcodeIdBecause", request.StreetcodeId].Value;
                        _logger.LogError(request, errorMessage);
                        return Result.Fail<TextDTO>(new Error(errorMessage));
                    }

                    var result = new NullResult<TextDTO>();

                    if (text is not null)
                    {
                        result.WithValue(_mapper.Map<TextDTO>(text));
                    }

                    return result;
                },
                TimeSpan.FromMinutes(10));
    }
}
