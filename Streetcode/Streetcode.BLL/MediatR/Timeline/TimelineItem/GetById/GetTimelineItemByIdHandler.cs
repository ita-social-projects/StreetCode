using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Timeline;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Timeline.TimelineItem.GetById;

public class GetTransactionLinkByIdHandler : IRequestHandler<GetTimelineItemByIdQuery, Result<TimelineItemDTO>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public GetTransactionLinkByIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
    }

    public async Task<Result<TimelineItemDTO>> Handle(GetTimelineItemByIdQuery request, CancellationToken cancellationToken)
    {
        var timelineItem = await _repositoryWrapper.TimelineRepository
            .GetFirstOrDefaultAsync(f => f.Id == request.Id);

        if (timelineItem is null)
        {
            return Result.Fail(new Error($"Cannot find a timeline item with corresponding id: {request.Id}"));
        }

        var timelineItemDto = _mapper.Map<TimelineItemDTO>(timelineItem);
        return Result.Ok(timelineItemDto);
    }
}