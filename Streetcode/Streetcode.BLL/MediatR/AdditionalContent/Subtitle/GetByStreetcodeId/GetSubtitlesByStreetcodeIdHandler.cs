using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.AdditionalContent.Subtitles;
using Streetcode.BLL.MediatR.AdditionalContent.Subtitle.GetById;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.AdditionalContent.Subtitle.GetByStreetcodeId
{
    public class GetSubtitlesByStreetcodeIdHandler : IRequestHandler<GetSubtitlesByStreetcodeIdQuery, Result<SubtitleDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IStringLocalizer<GetSubtitlesByStreetcodeIdHandler> _stringLocalizer;

        public GetSubtitlesByStreetcodeIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, IStringLocalizer<GetSubtitlesByStreetcodeIdHandler> stringLocalizer)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _stringLocalizer = stringLocalizer;
        }

        public async Task<Result<SubtitleDTO>> Handle(GetSubtitlesByStreetcodeIdQuery request, CancellationToken cancellationToken)
        {
            var subtitle = await _repositoryWrapper.SubtitleRepository
                .GetFirstOrDefaultAsync(Subtitle => Subtitle.StreetcodeId == request.StreetcodeId);

            if (subtitle is null)
            {
                return Result.Fail(new Error(_stringLocalizer $"Cannot find any subtitle by the streetcode id: {request.StreetcodeId}"));
            }

            var subtitleDto = _mapper.Map<SubtitleDTO>(subtitle);
            return Result.Ok(subtitleDto);
        }
    }
}
