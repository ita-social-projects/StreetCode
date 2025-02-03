using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Team.Position.GetByTitle;

public class GetByTitleTeamPositionHandler : IRequestHandler<GetByTitleTeamPositionQuery, Result<PositionDto>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repository;
    private readonly ILoggerService _loggerService;
    private readonly IStringLocalizer<CannotFindSharedResource> _localizer;

    public GetByTitleTeamPositionHandler(IMapper mapper, IRepositoryWrapper repository, ILoggerService loggerService, IStringLocalizer<CannotFindSharedResource> localizer)
    {
        _mapper = mapper;
        _repository = repository;
        _loggerService = loggerService;
        _localizer = localizer;
    }

    public async Task<Result<PositionDto>> Handle(GetByTitleTeamPositionQuery request, CancellationToken cancellationToken)
    {
        var position = await _repository.PositionRepository.GetFirstOrDefaultAsync(j => j.Position == request.position);

        if (position is null)
        {
            string exceptionMessege = _localizer["CannotFindPositionWithCorrespondingTitle", request.position];
            _loggerService.LogError(request, exceptionMessege);
            return Result.Fail(exceptionMessege);
        }

        try
        {
            var positionDto = _mapper.Map<PositionDto>(position);
            return Result.Ok(positionDto);
        }
        catch (Exception ex)
        {
            _loggerService.LogError(request, ex.Message);
            return Result.Fail(ex.Message);
        }
    }
}
