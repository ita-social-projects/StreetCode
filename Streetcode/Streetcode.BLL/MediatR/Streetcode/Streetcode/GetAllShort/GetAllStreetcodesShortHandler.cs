using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.GetAllShort;

public class GetAllStreetcodesShortHandler
    : IRequestHandler<GetAllStreetcodesShortQuery, Result<GetAllStreetcodesShortDto>>
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

    public Task<Result<GetAllStreetcodesShortDto>> Handle(
        GetAllStreetcodesShortQuery request,
        CancellationToken cancellationToken)
    {
        var paginatedStreetcodesShort = _repositoryWrapper.StreetcodeRepository.GetAllPaginated(request.page, request.pageSize);

        if (!paginatedStreetcodesShort.Entities.Any())
        {
            var errorMsg = _stringLocalizerNo["NoStreetcodesExistNow"].Value;
            _logger.LogError(request, errorMsg);
            return Task.FromResult(Result.Fail<GetAllStreetcodesShortDto>(errorMsg));
        }

        var getAllStreetcodeShortDto = new GetAllStreetcodesShortDto()
        {
            TotalAmount = paginatedStreetcodesShort.TotalItems,
            StreetcodesShort = _mapper.Map<IEnumerable<StreetcodeShortDto>>(paginatedStreetcodesShort.Entities),
        };

        return Task.FromResult(Result.Ok(getAllStreetcodeShortDto));
    }
}
