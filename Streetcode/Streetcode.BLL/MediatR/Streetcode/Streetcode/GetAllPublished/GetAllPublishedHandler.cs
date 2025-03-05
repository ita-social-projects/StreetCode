using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.GetAllPublished;

public class GetAllPublishedHandler : IRequestHandler<GetAllPublishedQuery,
    Result<IEnumerable<StreetcodeShortDTO>>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService _logger;
    private readonly IStringLocalizer<NoSharedResource> _stringLocalizerNo;

    public GetAllPublishedHandler(
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

    public async Task<Result<IEnumerable<StreetcodeShortDTO>>> Handle(GetAllPublishedQuery request, CancellationToken cancellationToken)
    {
        var streetcodes = await _repositoryWrapper.StreetcodeRepository.GetAllAsync(
            predicate: sc => sc.Status == DAL.Enums.StreetcodeStatus.Published);

        if (streetcodes.IsNullOrEmpty())
        {
            var errorMsg = _stringLocalizerNo["NoStreetcodesExistNow"].Value;
            _logger.LogError(request, errorMsg);
            return Result.Fail(errorMsg);
        }

        return Result.Ok(_mapper.Map<IEnumerable<StreetcodeShortDTO>>(streetcodes));
    }
}