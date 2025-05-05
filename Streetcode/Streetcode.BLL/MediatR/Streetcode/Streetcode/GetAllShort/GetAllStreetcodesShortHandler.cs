using System.Linq.Expressions;
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

    public async Task<Result<GetAllStreetcodesShortDto>> Handle(GetAllStreetcodesShortQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<StreetcodeContent, bool>>? basePredicate = null;
        var predicate = basePredicate.ExtendWithAccessPredicate(new StreetcodeAccessManager(), request.UserRole);

        var paginatedStreetcodesShort = _repositoryWrapper.StreetcodeRepository
            .GetAllPaginated(request.page, request.pageSize, predicate: predicate);

        if (!paginatedStreetcodesShort.Entities.Any())
        {
            var errorMsg = _stringLocalizerNo["NoStreetcodesExistNow"].Value;
            _logger.LogError(request, errorMsg);
            return Result.Fail<GetAllStreetcodesShortDto>(errorMsg);
        }

        var resultDto = new GetAllStreetcodesShortDto
        {
            TotalAmount = paginatedStreetcodesShort.TotalItems,
            StreetcodesShort = _mapper.Map<IEnumerable<StreetcodeShortDto>>(paginatedStreetcodesShort.Entities),
        };

        return Result.Ok(resultDto);
    }
}