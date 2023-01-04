using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.AdditionalContent.Subtitles;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Subtitle.GetById;

public class GetSubtitleByIdHandler : IRequestHandler<GetSubtitleByIdQuery, Result<SubtitleDTO>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public GetSubtitleByIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
    }

    public async Task<Result<SubtitleDTO>> Handle(GetSubtitleByIdQuery request, CancellationToken cancellationToken)
    {
        var subtitle = await _repositoryWrapper.SubtitleRepository.GetFirstOrDefaultAsync(f => f.Id == request.Id);

        if (subtitle is null)
        {
            return Result.Fail(new Error($"Cannot find a subtitle with corresponding Id: {request.Id}"));
        }

        var subtitleDto = _mapper.Map<SubtitleDTO>(subtitle);
        return Result.Ok(subtitleDto);
    }
}