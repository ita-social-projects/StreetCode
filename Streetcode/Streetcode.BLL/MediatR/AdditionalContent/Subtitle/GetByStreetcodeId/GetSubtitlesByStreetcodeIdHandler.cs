using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.AdditionalContent.Subtitles;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.AdditionalContent.Subtitle.GetByStreetcodeId
{
    public class GetSubtitlesByStreetcodeIdHandler : IRequestHandler<GetSubtitlesByStreetcodeIdQuery, Result<SubtitleDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;

        public GetSubtitlesByStreetcodeIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
        }

        public async Task<Result<SubtitleDTO>> Handle(GetSubtitlesByStreetcodeIdQuery request, CancellationToken cancellationToken)
        {
            var subtitle = await _repositoryWrapper.SubtitleRepository
                .GetFirstOrDefaultAsync(Subtitle => Subtitle.StreetcodeId == request.StreetcodeId);

            if (subtitle is null)
            {
                return Result.Fail(new Error($"Cannot find any subtitle by the streetcode id: {request.StreetcodeId}"));
            }

            var subtitleDto = _mapper.Map<SubtitleDTO>(subtitle);
            return Result.Ok(subtitleDto);
        }
    }
}
