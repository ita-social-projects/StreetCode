using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.GetShortById;

public class GetStreetcodeShortByIdHandler : IRequestHandler<GetStreetcodeShortByIdQuery, Result<StreetcodeShortDTO>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repository;
    private readonly ILoggerService _logger;
    private readonly IStringLocalizer<CannotMapSharedResource> _stringLocalizerCannotMap;

    public GetStreetcodeShortByIdHandler(
        IMapper mapper,
        IRepositoryWrapper repository,
        ILoggerService logger,
        IStringLocalizer<CannotMapSharedResource> stringLocalizerCannotMap)
    {
        _mapper = mapper;
        _repository = repository;
        _logger = logger;
        _stringLocalizerCannotMap = stringLocalizerCannotMap;
    }

    public async Task<Result<StreetcodeShortDTO>> Handle(GetStreetcodeShortByIdQuery request, CancellationToken cancellationToken)
    {
        var streetcode = await _repository.StreetcodeRepository.GetFirstOrDefaultAsync(st => st.Id == request.Id);
        var streetcodeShortDto = _mapper.Map<StreetcodeShortDTO>(streetcode);

        if (streetcodeShortDto == null)
        {
            var errorMsg = _stringLocalizerCannotMap["CannotMapStreetcodeToShortDTO"].Value;
            _logger.LogError(request, errorMsg);
            return Result.Fail(new Error(errorMsg));
        }

        return Result.Ok(streetcodeShortDto);
    }
}