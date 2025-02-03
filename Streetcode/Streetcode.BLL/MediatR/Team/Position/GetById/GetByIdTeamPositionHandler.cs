using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Team.Position.GetById;

public class GetByIdTeamPositionHandler : IRequestHandler<GetByIdTeamPositionQuery, Result<PositionDto>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repository;
    private readonly ILoggerService _loggerService;
    private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizer;

    public GetByIdTeamPositionHandler(IMapper mapper, IRepositoryWrapper repository, ILoggerService loggerService, IStringLocalizer<CannotFindSharedResource> localizer)
    {
        _mapper = mapper;
        _repository = repository;
        _loggerService = loggerService;
        _stringLocalizer = localizer;
    }

    public async Task<Result<PositionDto>> Handle(GetByIdTeamPositionQuery request, CancellationToken cancellationToken)
    {
        var position = await _repository.PositionRepository.GetFirstOrDefaultAsync(j => j.Id == request.positionId);

        if (position is null)
        {
            string exceptionMessege = _stringLocalizer["CannotFindPositionWithCorrespondingId", request.positionId];
            _loggerService.LogError(request, exceptionMessege);
            return Result.Fail(exceptionMessege);
        }

        try
        {
            var positionDTO = _mapper.Map<PositionDto>(position);
            return Result.Ok(positionDTO);
        }
        catch (Exception ex)
        {
            _loggerService.LogError(request, ex.Message);
            return Result.Fail(ex.Message);
        }
    }
}
