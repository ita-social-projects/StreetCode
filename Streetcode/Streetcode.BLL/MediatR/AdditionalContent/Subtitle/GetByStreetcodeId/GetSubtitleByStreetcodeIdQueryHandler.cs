using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.AdditionalContent.Subtitles;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.AdditionalContent.Subtitle.GetByStreetcodeId
{
    public class GetSubtitleByStreetcodeIdQueryHandler : IRequestHandler<GetSubtitleByStreetcodeIdQuery, Result<SubtitleDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;

        public GetSubtitleByStreetcodeIdQueryHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
        }

        public async Task<Result<SubtitleDTO>> Handle(GetSubtitleByStreetcodeIdQuery request, CancellationToken cancellationToken)
        {
            var subtitle = await _repositoryWrapper.SubtitleRepository.GetFirstOrDefaultAsync(Subtitle => Subtitle.StreetcodeId == request.streetcodeId);
            if (subtitle is null)
            {
                return Result.Fail(new Error($"Cannot find a subtitle by a streetcode id: {request.streetcodeId}"));
            }

            var subtitleDto = _mapper.Map<SubtitleDTO>(subtitle);
            return Result.Ok(subtitleDto);
        }
    }
}
