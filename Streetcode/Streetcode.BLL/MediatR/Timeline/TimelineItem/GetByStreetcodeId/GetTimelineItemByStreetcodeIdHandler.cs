using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Timeline;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Timeline.TimelineItem.GetByStreetcodeId;

public class GetTransactLinkByStreetcodeIdHandler : IRequestHandler<GetTransactLinkByStreetcodeIdQuery, Result<TimelineItemDTO>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public GetTransactLinkByStreetcodeIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
    }

    public async Task<Result<TimelineItemDTO>> Handle(GetTransactLinkByStreetcodeIdQuery request, CancellationToken cancellationToken)
    {
        var timelineItem = await _repositoryWrapper.TimelineRepository
            .GetSingleOrDefaultAsync(f => f.Streetcodes.Any(s => s.Id == request.StreetcodeId));

        if (timelineItem is null)
        {
            return Result.Fail(new Error($"Cannot find a fact by a streetcode Id: {request.StreetcodeId}"));
        }

        var timelineItemDto = _mapper.Map<TimelineItemDTO>(timelineItem);
        return Result.Ok(timelineItemDto);
    }
}