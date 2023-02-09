using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.AdditionalContent.Subtitles;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.AdditionalContent.Subtitle.GetByStreetcodeId
{
    public class GetSubtitlesByStreetcodeIdHandler : IRequestHandler<GetSubtitlesByStreetcodeIdQuery, Result<IEnumerable<SubtitleDTO>>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;

        public GetSubtitlesByStreetcodeIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<SubtitleDTO>>> Handle(GetSubtitlesByStreetcodeIdQuery request, CancellationToken cancellationToken)
        {
            if ((await _repositoryWrapper.StreetcodeRepository.GetFirstOrDefaultAsync(s => s.Id == request.StreetcodeId)) is null)
            {
                return Result.Fail(
                    new Error($"Cannot find a subtitles by a streetcode id: {request.StreetcodeId}, because such streetcode doesn`t exist"));
            }

            var subtitles = await _repositoryWrapper.SubtitleRepository
                .GetAllAsync(Subtitle => Subtitle.StreetcodeId == request.StreetcodeId);

            if (subtitles is null)
            {
                return Result.Fail(new Error($"Cannot find any subtitle by the streetcode id: {request.StreetcodeId}"));
            }

            var subtitlesDto = _mapper.Map<IEnumerable<SubtitleDTO>>(subtitles);
            return Result.Ok(subtitlesDto);
        }
    }
}
