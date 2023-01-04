using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.AdditionalContent.Subtitles;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Subtitle.GetAll;

public class GetAllSubtitlesHandler : IRequestHandler<GetAllSubtitlesQuery, Result<IEnumerable<SubtitleDTO>>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public GetAllSubtitlesHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<SubtitleDTO>>> Handle(GetAllSubtitlesQuery request, CancellationToken cancellationToken)
    {
        var subtitles = await _repositoryWrapper.SubtitleRepository.GetAllAsync();

        var subtitleDtos = _mapper.Map<IEnumerable<SubtitleDTO>>(subtitles);
        return Result.Ok(subtitleDtos);
    }
}