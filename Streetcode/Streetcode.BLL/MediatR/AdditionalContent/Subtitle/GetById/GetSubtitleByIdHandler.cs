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
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService? _logger = null;

    public GetSubtitleByIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService? logger = null)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<SubtitleDTO>> Handle(GetSubtitleByIdQuery request, CancellationToken cancellationToken)
    {
        var subtitle = await _repositoryWrapper.SubtitleRepository.GetFirstOrDefaultAsync(f => f.Id == request.Id);

        if (subtitle is null)
        {
            string errorMsg = $"Cannot find a subtitle with corresponding id: {request.Id}";
            _logger?.LogError("GetSubtitleByIdQuery handled with an error");
            _logger?.LogError(errorMsg);
            return Result.Fail(new Error(errorMsg));
        }

        _logger?.LogInformation("GetSubtitleByIdQuery handled successfully");
        var subtitleDto = _mapper.Map<SubtitleDTO>(subtitle);
        return Result.Ok(subtitleDto);
    }
}