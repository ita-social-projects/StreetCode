using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.AdditionalContent.Subtitles;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.AdditionalContent.Subtitle.GetAll;

public class GetAllSubtitlesHandler : IRequestHandler<GetAllSubtitlesQuery, Result<IEnumerable<SubtitleDTO>>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService _logger;

    public GetAllSubtitlesHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<IEnumerable<SubtitleDTO>>> Handle(GetAllSubtitlesQuery request, CancellationToken cancellationToken)
    {
        var subtitles = await _repositoryWrapper.SubtitleRepository.GetAllAsync();

        if (subtitles is null)
        {
            const string errorMsg = $"Cannot find any subtitles";

            _logger?.LogError("GetAllSubtitlesQuery handled with an error");
            _logger?.LogError(errorMsg);

            return Result.Fail(new Error(errorMsg));
        }

        var subtitleDtos = _mapper.Map<IEnumerable<SubtitleDTO>>(subtitles);
        _logger?.LogInformation($"GetAllSubtitlesQuery handled successfully. Retrieved {subtitleDtos.Count()} subtitles");
        return Result.Ok(subtitleDtos);
    }
}