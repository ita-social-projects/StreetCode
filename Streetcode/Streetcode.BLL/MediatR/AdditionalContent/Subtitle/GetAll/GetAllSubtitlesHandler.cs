using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.AdditionalContent.Subtitles;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.AdditionalContent.Subtitle.GetAll;

public class GetAllSubtitlesHandler : IRequestHandler<GetAllSubtitlesQuery, Result<IEnumerable<SubtitleDTO>>>
{
    private readonly ILoggerService _loggerService;
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public GetAllSubtitlesHandler(
        IRepositoryWrapper repositoryWrapper,
        IMapper mapper,
        ILoggerService loggerService)
    {
        _loggerService = loggerService;
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<SubtitleDTO>>> Handle(GetAllSubtitlesQuery request, CancellationToken cancellationToken)
    {
        _loggerService.LogInformation("Entry into GetAllSubtitlesHandler");

        var subtitles = await _repositoryWrapper.SubtitleRepository.GetAllAsync();

        if (subtitles is null)
        {
            return Result.Fail(new Error($"Cannot find any subtitles"));
        }

        var subtitleDtos = _mapper.Map<IEnumerable<SubtitleDTO>>(subtitles);
        return Result.Ok(subtitleDtos);
    }
}