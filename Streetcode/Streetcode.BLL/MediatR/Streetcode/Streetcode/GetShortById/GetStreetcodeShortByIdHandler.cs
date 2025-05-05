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

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.GetShortById;

public class GetStreetcodeShortByIdHandler : IRequestHandler<GetStreetcodeShortByIdQuery, Result<StreetcodeShortDto>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repository;
    private readonly ILoggerService _logger;
    private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;
    private readonly IStringLocalizer<CannotMapSharedResource> _stringLocalizerCannotMap;

    public GetStreetcodeShortByIdHandler(
        IMapper mapper,
        IRepositoryWrapper repository,
        ILoggerService logger,
        IStringLocalizer<CannotMapSharedResource> stringLocalizerCannotMap,
        IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind)
    {
        _mapper = mapper;
        _repository = repository;
        _logger = logger;
        _stringLocalizerCannotMap = stringLocalizerCannotMap;
        _stringLocalizerCannotFind = stringLocalizerCannotFind;
    }

    public async Task<Result<StreetcodeShortDto>> Handle(GetStreetcodeShortByIdQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<StreetcodeContent, bool>>? basePredicate = st => st.Id == request.Id;
        var predicate = basePredicate.ExtendWithAccessPredicate(new StreetcodeAccessManager(), request.UserRole);

        var streetcode = await _repository.StreetcodeRepository.GetFirstOrDefaultAsync(st => st.Id == request.Id);

        if (streetcode == null)
        {
            string errorMsg = _stringLocalizerCannotFind["CannotFindStreetcodeById"].Value;
            _logger.LogError(request, errorMsg);
            return Result.Fail(new Error(errorMsg));
        }

        var streetcodeShortDto = _mapper.Map<StreetcodeShortDto>(streetcode);

        if (streetcodeShortDto == null)
        {
            var errorMsg = _stringLocalizerCannotMap["CannotMapStreetcodeToShortDTO"].Value;
            _logger.LogError(request, errorMsg);
            return Result.Fail(new Error(errorMsg));
        }

        return Result.Ok(streetcodeShortDto);
    }
}