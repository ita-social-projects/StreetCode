using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.AdditionalContent.Subtitles;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.AdditionalContent.Subtitle.GetAll;

public class GetAllSubtitlesHandler : IRequestHandler<GetAllSubtitlesQuery, Result<IEnumerable<SubtitleDTO>>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IStringLocalizer<GetAllSubtitlesHandler> _stringLocalizer;

    public GetAllSubtitlesHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, IStringLocalizer<GetAllSubtitlesHandler> stringLocalizer)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _stringLocalizer = stringLocalizer;
    }

    public async Task<Result<IEnumerable<SubtitleDTO>>> Handle(GetAllSubtitlesQuery request, CancellationToken cancellationToken)
    {
        var subtitles = await _repositoryWrapper.SubtitleRepository.GetAllAsync();

        if (subtitles is null)
        {
            return Result.Fail(new Error(_stringLocalizer?["CannotFindAnySubtitles"].Value));
        }

        var subtitleDtos = _mapper.Map<IEnumerable<SubtitleDTO>>(subtitles);
        return Result.Ok(subtitleDtos);
    }
}