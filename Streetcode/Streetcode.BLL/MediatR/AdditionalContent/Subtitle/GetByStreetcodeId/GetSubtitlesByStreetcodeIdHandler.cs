using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.AdditionalContent.Subtitles;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.AdditionalContent.Subtitle.GetByStreetcodeId
{
    public class GetSubtitlesByStreetcodeIdHandler : IRequestHandler<GetSubtitlesByStreetcodeIdQuery, Result<SubtitleDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;

        public GetSubtitlesByStreetcodeIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService? logger = null)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<SubtitleDTO>> Handle(GetSubtitlesByStreetcodeIdQuery request, CancellationToken cancellationToken)
        {
            var subtitle = await _repositoryWrapper.SubtitleRepository
                .GetFirstOrDefaultAsync(Subtitle => Subtitle.StreetcodeId == request.StreetcodeId);

            if (subtitle is null)
            {
                string errorMsg = $"Cannot find any subtitle by the streetcode id: {request.StreetcodeId}";
                _logger?.LogError("GetSubtitlesByStreetcodeIdQuery handled with an error");
                _logger?.LogError(errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            _logger?.LogInformation($"GetSubtitlesByStreetcodeIdQuery handled successfully");
            var subtitleDto = _mapper.Map<SubtitleDTO>(subtitle);
            return Result.Ok(subtitleDto);
        }
    }
}
