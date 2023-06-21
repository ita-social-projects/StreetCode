using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.AdditionalContent.Subtitles;
using Streetcode.BLL.MediatR.AdditionalContent.GetById;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.AdditionalContent.Subtitle.GetById;

public class GetSubtitleByIdHandler : IRequestHandler<GetSubtitleByIdQuery, Result<SubtitleDTO>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IStringLocalizer<GetSubtitleByIdHandler> _stringLocalizer;

    public GetSubtitleByIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, IStringLocalizer<GetSubtitleByIdHandler> stringLocalizer)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _stringLocalizer = stringLocalizer;
    }

    public async Task<Result<SubtitleDTO>> Handle(GetSubtitleByIdQuery request, CancellationToken cancellationToken)
    {
        var subtitle = await _repositoryWrapper.SubtitleRepository.GetFirstOrDefaultAsync(f => f.Id == request.Id);

        if (subtitle is null)
        {
            return Result.Fail(_stringLocalizer["CannotFindSubtitleWithCorrespondingId", request.Id]);
        }

        var subtitleDto = _mapper.Map<SubtitleDTO>(subtitle);
        return Result.Ok(subtitleDto);
    }
}