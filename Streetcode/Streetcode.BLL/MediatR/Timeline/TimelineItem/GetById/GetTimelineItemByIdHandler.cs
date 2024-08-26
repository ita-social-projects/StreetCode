using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Timeline;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Timeline.TimelineItem.GetById;

public class GetTimelineItemByIdHandler : IRequestHandler<GetTimelineItemByIdQuery, Result<TimelineItemDTO>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService _logger;
    private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;

    public GetTimelineItemByIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger, IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _logger = logger;
        _stringLocalizerCannotFind = stringLocalizerCannotFind;
    }

    public async Task<Result<TimelineItemDTO>> Handle(GetTimelineItemByIdQuery request, CancellationToken cancellationToken)
    {
        var timelineItem = await _repositoryWrapper.TimelineRepository
            .GetFirstOrDefaultAsync(
                predicate: ti => true,
                include: ti => ti
                    .Include(til => til.HistoricalContextTimelines)
                        .ThenInclude(x => x.HistoricalContext) !);

        if (timelineItem is null)
        {
            string errorMsg = _stringLocalizerCannotFind["CannotFindTimelineItemWithCorrespondingId", request.Id].Value;
            _logger.LogError(request, errorMsg);
            return Result.Fail(new Error(errorMsg));
        }

        return Result.Ok(_mapper.Map<TimelineItemDTO>(timelineItem));
    }
}
