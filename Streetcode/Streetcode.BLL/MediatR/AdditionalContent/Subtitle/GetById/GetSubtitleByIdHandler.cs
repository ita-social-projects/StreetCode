using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.AdditionalContent.Subtitles;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.AdditionalContent.GetById;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.AdditionalContent.Subtitle.GetById;

public class GetSubtitleByIdHandler : IRequestHandler<GetSubtitleByIdQuery, Result<SubtitleDTO>>
{
    private readonly ILoggerService _loggerService;
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public GetSubtitleByIdHandler(
        IRepositoryWrapper repositoryWrapper,
        IMapper mapper,
        ILoggerService loggerService)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _loggerService = loggerService;
    }

    public async Task<Result<SubtitleDTO>> Handle(GetSubtitleByIdQuery request, CancellationToken cancellationToken)
    {
        _loggerService.LogInformation("Entry into GetSubtitleByIdHandler");
        var subtitle = await _repositoryWrapper.SubtitleRepository.GetFirstOrDefaultAsync(f => f.Id == request.Id);

        if (subtitle is null)
        {
            return Result.Fail(new Error($"Cannot find a subtitle with corresponding id: {request.Id}"));
        }

        var subtitleDto = _mapper.Map<SubtitleDTO>(subtitle);
        return Result.Ok(subtitleDto);
    }
}